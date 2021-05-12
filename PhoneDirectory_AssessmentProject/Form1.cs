using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PhoneDirectory_AssessmentProject.Models;

namespace PhoneDirectory_AssessmentProject
{
    public partial class Form1 : Form
    {
        AppDataContext mContext;

        string mName;
        string mAddress;
        string mPhone;


        public Form1()
        {
            InitializeComponent();

            mContext = new AppDataContext();

            populateGrid();
        }


        private void getData()
        {
            mName = nameTB.Text;
            mAddress = addressTB.Text;
            mPhone = "";
        }


        private void addBtn_Click(object sender, EventArgs e)
        {
            getData();

            if (Regex.IsMatch(phoneTB.Text, @"^\d+$"))
            {
                mPhone = phoneTB.Text;

                var entry = new PhoneDirectory() { Name = mName, Phone = mPhone, Address = mAddress };

                if (writeEntry(entry))
                {
                    displayMessageBox("Success", "New entry added!");
                    RefreshTxtBox();
                    populateGrid();
                }
                else
                    displayMessageBox("Error", "Unable to add entry.");
            }
            else
                displayMessageBox("Input Error", "Incorrect data entered");   
        }


        private void search()
        {
            getData();

            mPhone = phoneTB.Text;

            string condition = "SELECT * FROM dbo.PhoneDirectories";

            if (mName == "" && mPhone == "" && mAddress == "")
                displayMessageBox("Invalid Input", "Please enter search constraints.");
            else
            {
                condition = condition + " WHERE";

                if (mName != "")
                    condition = condition + " Name = '" + mName + "' AND";
                if (mPhone != "")
                    condition = condition + " Phone = '" + mPhone + "' AND";
                if (mAddress != "")
                    condition = condition + " Address = '" + mAddress + "' AND";

                condition += " 1=1";
            }
            
            try
            {
                var data = mContext.PhoneDirectories.SqlQuery(condition).ToList();
                dataGridView1.DataSource = data;
                RefreshTxtBox();
            }
            catch (Exception ex)
            {
                displayMessageBox("Exception Caught", ex.Message);
            }
        }


        private bool writeEntry(PhoneDirectory entry)
        {
            bool flag = false;

            try
            {
                mContext.PhoneDirectories.Add(entry);
                mContext.SaveChanges();
                flag = true;
                RefreshTxtBox();
            }
            catch (Exception ex)
            {
                displayMessageBox("Exception Caught", ex.Message);
            }

            return flag;
        }


        private void populateGrid()
        {
            try
            {
                var dataEntries = mContext.PhoneDirectories.ToList();
                dataGridView1.DataSource = dataEntries;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void displayMessageBox(string message, string title)
        {
            MessageBox.Show(title, message);
        }


        private void searchBtn_Click_1(object sender, EventArgs e)
        {
            search();
        }


        private void updateBtn_Click(object sender, EventArgs e)
        {
            if ((idTB.Text != "") && (Regex.IsMatch(idTB.Text, @"^\d+$")))
            {
                int id = Int32.Parse(idTB.Text);

                getData();
                mPhone = phoneTB.Text;

                PhoneDirectory someEntry = new PhoneDirectory();
                someEntry = mContext.PhoneDirectories.Find(id);

                if (someEntry != null)
                {
                    try
                    {
                        if (mName != "") someEntry.Name = mName;
                        if (mPhone != "") someEntry.Phone = mPhone;
                        if (mAddress != "") someEntry.Address = mAddress;

                        mContext.SaveChanges();
                        displayMessageBox("Success", "Entry updated");
                        RefreshTxtBox();
                        populateGrid();
                    }
                    catch (Exception ex)
                    {
                        displayMessageBox("Exception Caught", ex.Message);
                    }
                }
                else
                    displayMessageBox("Improper Input", "ID not found");
            }
            else
                displayMessageBox("Input Error", "Please enter valid inpit.");
        }


        private void RefreshTxtBox()
        {
            idTB.Text = "";
            mName = "";
            mPhone= "";
            mAddress = "";
        }


        private void updateEntryBtn_Click(object sender, EventArgs e)
        {
            makeVisible();
            addBtn.Visible = false;
            searchBtn.Visible = false;
        }


        private void makeVisible()
        {
            idTB.Visible = true;
            idLbl.Visible = true;
            updateBtn.Visible = true;
            backBtn.Visible = true;

        }


        private void backBtn_Click(object sender, EventArgs e)
        {
            makeInvisible();
        }


        private void makeInvisible()
        {
            idTB.Visible = false;
            idLbl.Visible = false;
            updateBtn.Visible = false;
            backBtn.Visible = false;

        }
    }
}
