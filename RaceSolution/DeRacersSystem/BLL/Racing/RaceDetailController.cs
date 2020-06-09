using DeRacersSystem.DAL;
using DeRacersSystem.Data.Entities;
using DeRacersSystem.Data.RacingPOCOs;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.MobileControls;

namespace DeRacersSystem.BLL.Racing
{
    [DataObject]
    public class RaceDetailController
    {
        List<string> errors = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RosterView> Get_RosterView(int raceid)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.RaceDetails
                              where x.RaceID == raceid
                              && x.Refund == false
                              select new RosterView
                              {
                                  RaceDetailID = x.RaceDetailID,
                                  Name = x.Member.FirstName + " " + x.Member.LastName,
                                  RaceFee = x.RaceFee,
                                  CarID = x.CarID,
                                  RentalFee = x.RentalFee,
                                  Placement = x.Place,
                                  Refunded = x.Refund,
                                  Comment = x.Comment,
                                  RefundReason = x.RefundReason
                              };
                return results.ToList();
            }
        }

        public List<RaceResultsView> Get_RaceResultsView(int raceid)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.RaceDetails
                              where x.RaceID == raceid
                              && x.Refund == false
                              select new RaceResultsView
                              {
                                  RaceDetailID = x.RaceDetailID,
                                  Name = x.Member.FirstName + " " + x.Member.LastName,
                                  Time = x.RunTime,
                                  Penalties = x.RacePenalty.PenaltyID == null ? 0 : x.RacePenalty.PenaltyID
                              };
                return results.ToList();
            }
        }

        public int Update_RosterView(int employeeid, int raceid, RosterView item)
        {
            using (var context = new RaceContext())
            {
                RaceDetail newitem = new RaceDetail();
                Invoice newinvoice = new Invoice();
                if (item.CarID != 0 && !(from x in context.Cars
                                         where x.CarClass.CertificationLevel == (from y in context.RaceDetails
                                                                                 where y.RaceDetailID == item.RaceDetailID
                                                                                 select y.Race.CertificationLevel).FirstOrDefault()
                                         select x.CarID).Contains((int)item.CarID))
                {
                    errors.Add("CarID must have proper certification level.");
                }
                if (item.Refunded)
                {
                    if (string.IsNullOrEmpty(item.RefundReason))
                    {
                        errors.Add("Refunds require a refund reason.");
                    }
                    else
                    {
                        newitem = new RaceDetail
                        {
                            RaceDetailID = item.RaceDetailID,
                            RaceID = raceid,
                            MemberID = (from x in context.Members
                                        where x.FirstName + " " + x.LastName == item.Name
                                        select x.MemberID).FirstOrDefault(),
                            RaceFee = 0,
                            CarID = null,
                            RentalFee = 0,
                            Place = null,
                            Refund = true,
                            Comment = item.Comment,
                            RefundReason = item.RefundReason
                        };

                        decimal subtotal = item.RaceFee;
                        newinvoice = new Invoice
                        {
                            InvoiceDate = DateTime.Now.Date,
                            EmployeeID = employeeid,
                            SubTotal = subtotal,
                            GST = subtotal * (decimal)0.05,
                            Total = subtotal * (decimal)1.05
                        };
                        context.Invoices.Add(newinvoice);
                    }
                }
                else
                {
                    newitem = (from x in context.RaceDetails
                               where x.RaceDetailID == item.RaceDetailID
                               select x).FirstOrDefault();
                    newitem.CarID = item.CarID == 0 ? null : item.CarID;
                    newitem.RentalFee = item.CarID == 0 ? 0 : (from x in context.CarClasses
                                                               where x.CarClassID == (from y in context.Cars
                                                                                      where y.CarID == item.CarID
                                                                                      select y.CarClassID).FirstOrDefault()
                                                               select x.RaceRentalFee).FirstOrDefault();
                    newitem.Refund = false;
                    newitem.Comment = item.Comment;
                    newitem.RefundReason = item.RefundReason;
                }

                if (errors.Count == 0)
                {
                    context.Entry(newitem).State = System.Data.Entity.EntityState.Modified;
                    return context.SaveChanges();
                }
                else
                {
                    throw new BusinessRuleException("Update Validation Error", errors);
                }

            }
        }

        public int Insert_RosterView(int employeeid, int raceid, RosterView item)
        {
            using (var context = new RaceContext())
            {
                RaceDetail newitem = new RaceDetail();
                Invoice newinvoice = new Invoice();
                if ((from x in context.RaceDetails
                     where x.RaceID == raceid
                     && x.Refund == false
                     select x).Count() >= (from x in context.Races
                                           where x.RaceID == raceid
                                           select x.NumberOfCars).FirstOrDefault())
                {
                    errors.Add("Race contestant limit has already been reached.");
                }
                if (item.Name == "0")
                {
                    errors.Add("Please select a driver.");
                }
                List<int> members = (from x in context.RaceDetails
                                     where x.RaceID == raceid
                                     && x.Refund == false
                                     select x.MemberID).ToList();
                if (members.Contains(int.Parse(item.Name)))
                {
                    errors.Add("Member cannot be entered in a race twice.");
                }
                string certif = (from y in context.Races
                                 where y.RaceID == raceid
                                 select y.CertificationLevel).FirstOrDefault();
                if (item.CarID != 0 && !(from x in context.Cars
                                         where x.CarClass.CertificationLevel == certif
                                         select x.CarID).Contains((int)item.CarID))
                {
                    errors.Add("CarID must have proper certification level.");
                }

                if (errors.Count == 0)
                {
                    newitem = new RaceDetail
                    {
                        RaceID = raceid,
                        MemberID = int.Parse(item.Name),
                        RaceFee = (decimal)(from x in context.RaceDetails
                                            where x.RaceID == raceid
                                            && x.RaceFee != 0
                                            select x.RaceFee).FirstOrDefault(),
                        CarID = item.CarID == 0 ? null : item.CarID,
                        RentalFee = item.CarID == 0 ? 0 : (from x in context.CarClasses
                                                           where x.CarClassID == (from y in context.Cars
                                                                                  where y.CarID == item.CarID
                                                                                  select y.CarClassID).FirstOrDefault()
                                                           select x.RaceRentalFee).FirstOrDefault(),
                        Place = item.Placement,
                        Refund = false,
                        Comment = item.Comment,
                        RefundReason = item.RefundReason
                    };
                    decimal subtotal = (from x in context.RaceDetails
                                        where x.RaceDetailID == item.RaceDetailID
                                        select x.Invoice.SubTotal).FirstOrDefault();
                    newinvoice = new Invoice
                    {
                        InvoiceDate = DateTime.Now.Date,
                        EmployeeID = employeeid,
                        SubTotal = subtotal,
                        GST = subtotal * (decimal)0.05,
                        Total = subtotal * (decimal)1.05
                    };

                    context.RaceDetails.Add(newitem);
                    context.Invoices.Add(newinvoice);

                    context.SaveChanges();
                    return newitem.RaceDetailID;
                }
                else
                {
                    throw new BusinessRuleException("Insert Validation Error", errors);
                }
            }
        }

        public int Update_RaceResultsView(int employeeid, int raceid, List<RaceResultsView> items)
        {
            using (var context = new RaceContext())
            {
                foreach (RaceResultsView item in items)
                {
                    if (item.Time == null)
                    {
                        errors.Add("Times must be in the format hh:mm:ss.");
                        break;
                    }
                }
                foreach (RaceResultsView item in items)
                {
                    if (item.Time != null & item.Time < new TimeSpan(00, 00, 00))
                    {
                        errors.Add("Times must be positive.");
                        break;
                    }
                }

                if (errors.Count == 0)
                {
                    items = items.OrderBy(x => x.Time).ToList();
                    int? placement = 0;
                    int? penalties = null;
                    foreach (RaceResultsView item in items)
                    {
                        if (item.Time != new TimeSpan(00, 00, 00))
                        {
                            placement++;
                        }
                        else
                        {
                            item.Time = null;
                        }
                        if (item.Penalties != 0)
                        {
                            penalties = item.Penalties;
                        }
                        RaceDetail newitem = (from x in context.RaceDetails
                                              where x.RaceDetailID == item.RaceDetailID
                                              select x).FirstOrDefault();
                        newitem.Place = placement == 0 ? null : placement;
                        newitem.RunTime = item.Time;
                        newitem.PenaltyID = penalties;
                        context.Entry(newitem).State = System.Data.Entity.EntityState.Modified;

                    }
                }
                if (errors.Count == 0)
                {
                    return context.SaveChanges();
                }
                else
                {
                    throw new BusinessRuleException("Race Time Validation Error.", errors);
                }
            }
        }
    }
}
