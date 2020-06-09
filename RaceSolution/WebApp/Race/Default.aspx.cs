using DeRacersSystem.BLL.Racing;
using DeRacersSystem.Data.RacingPOCOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.Race
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RaceCalendar_SelectionChanged(object sender, EventArgs e)
        {
            SchedulePanel.Visible = true;
            RosterPanel.Visible = false;
            RaceResultPanel.Visible = false;
        }

        protected void ScheduleGV_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            RaceID.Text = e.CommandArgument.ToString();
            RefreshRoster();
            RosterPanel.Visible = true;
            RaceResultPanel.Visible = false;
        }

        protected void RosterLV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int row = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString() == "EditContestant")
            {
                foreach (ListViewItem item in RosterLV.Items)
                {
                    (item.FindControl("EditView") as Panel).Visible = false;
                    (item.FindControl("ItemView") as Panel).Visible = true;
                }
                (RosterLV.Items[row].FindControl("EditView") as Panel).Visible = true;
                (RosterLV.Items[row].FindControl("ItemView") as Panel).Visible = false;
            }
            if (e.CommandName.ToString() == "UpdateContestant")
            {
                MessageUserControl.TryRun(() =>
                {
                    RaceDetailController rdsysmgr = new RaceDetailController();
                    RosterView newitem = new RosterView
                    {
                        RaceDetailID = int.Parse((RosterLV.Items[row].FindControl("ERaceDetailIDLabel") as Label).Text),
                        Name = (RosterLV.Items[row].FindControl("ENameLabel") as Label).Text,
                        RaceFee = 0,//decimal.Parse((RosterLV.Items[row].FindControl("ERaceFeeLabel") as Label).Text),
                        CarID = int.Parse((RosterLV.Items[row].FindControl("ECarDDL") as DropDownList).SelectedValue),
                        RentalFee = 0,
                        Placement = string.IsNullOrEmpty((RosterLV.Items[row].FindControl("PlacementLabel") as Label).Text) ? 0 : int.Parse((RosterLV.Items[row].FindControl("PlacementLabel") as Label).Text),
                        Refunded = (RosterLV.Items[row].FindControl("ERefundedCheckbox") as CheckBox).Checked,
                        Comment = (RosterLV.Items[row].FindControl("ECommentTextBox") as TextBox).Text,
                        RefundReason = (RosterLV.Items[row].FindControl("EReasonTextBox") as TextBox).Text
                    };
                    rdsysmgr.Update_RosterView(int.Parse(EmployeeDDL.SelectedValue), int.Parse(RaceID.Text), newitem);
                }, "Success", "Successfully updated contestant.");
                
                RefreshRoster();
            }
            if (e.CommandName.ToString() == "CancelContestant")
            {
                RefreshRoster();
            }
            if (e.CommandName.ToString() == "InsertContestant")
            {
                MessageUserControl.TryRun(() =>
                {
                    RaceDetailController rdsysmgr = new RaceDetailController();
                    RosterView newitem = new RosterView
                    {
                        Name = (RosterLV.InsertItem.FindControl("IDriverDDL") as DropDownList).SelectedValue,
                        RaceFee = decimal.Parse((RosterLV.Items[0].FindControl("ERaceFeeLabel") as Label).Text),
                        CarID = int.Parse((RosterLV.InsertItem.FindControl("ICarDDL") as DropDownList).SelectedValue),
                        RentalFee = 0,
                        Placement = null,
                        Refunded = false,
                        Comment = null,
                        RefundReason = null
                    };
                    rdsysmgr.Insert_RosterView(int.Parse(EmployeeDDL.SelectedValue), int.Parse(RaceID.Text), newitem);
                }, "Success", "Successfully inserted contestant.");
                RefreshRoster();
            }
        }

        protected void RecordRaceTimesButtonID_Click(object sender, EventArgs e)
        {
            RaceResultPanel.Visible = true;
        }

        protected void SaveRaceTimesButtonID_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                RaceDetailController rdsysmgr = new RaceDetailController();
                List<RaceResultsView> itemlist = new List<RaceResultsView>();
                foreach (GridViewRow item in RaceResultsGV.Rows)
                {
                    TimeSpan? parsedtime = new TimeSpan();
                    TimeSpan outtime = new TimeSpan();
                    if(string.IsNullOrEmpty((item.FindControl("TimeID") as TextBox).Text))
                    {
                        parsedtime = new TimeSpan(00,00,00);
                    }
                    else if (TimeSpan.TryParse((item.FindControl("TimeID") as TextBox).Text, out outtime))
                    {
                        parsedtime = outtime;
                    }
                    else
                    {
                        parsedtime = null;
                    }
                    RaceResultsView newitem = new RaceResultsView
                    {
                        RaceDetailID = int.Parse((item.FindControl("RaceDetailID") as Label).Text),
                        Name = (item.FindControl("NameID") as Label).Text,
                        Time = parsedtime,
                        Penalties = int.Parse((item.FindControl("PenaltyDDL") as DropDownList).SelectedValue)
                    };
                    itemlist.Add(newitem);
                }
                rdsysmgr.Update_RaceResultsView(int.Parse(EmployeeDDL.SelectedValue), int.Parse(RaceID.Text), itemlist);
            }, "Success", "Successfully saved race results.");
            RaceResultPanel.Visible = false;
            RefreshRoster();
        }

        private void RefreshRoster()
        {
            RaceDetailController rdcsysmgr = new RaceDetailController();
            RosterLV.DataSource = rdcsysmgr.Get_RosterView(int.Parse(RaceID.Text));
            RosterLV.InsertItemPosition = InsertItemPosition.LastItem;
            RosterLV.DataBind();
            string racecost = (RosterLV.Items.First().FindControl("RaceFeeLabel") as Label).Text;
            (RosterLV.InsertItem.FindControl("IRaceFeeLabel") as Label).Text = racecost;
            RosterLV.DataBind();
            foreach (ListViewItem item in RosterLV.Items)
            {
                (item.FindControl("EditView") as Panel).Visible = false;
                (item.FindControl("ItemView") as Panel).Visible = true;
            }

            RaceResultsGV.DataSource = rdcsysmgr.Get_RaceResultsView(int.Parse(RaceID.Text));
            RaceResultsGV.DataBind();
        }
    }
}