using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnrollmentSystem
{
    public partial class SubjectEntry : Form
    {
        public SubjectEntry()
        {
            InitializeComponent();
            SubjectDataGridView.CellFormatting += SubjectDataGridView_CellFormatting;
        }

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79286_CC_APPSDEV22_1030_1230_PM_MW\79286-23207764\Desktop\EnrollmentSystem-main\Bernabe.accdb";

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection thisConnection = new OleDbConnection(connectionString);
            string Ole = "Select * from SubjectFile";
            OleDbDataAdapter thisAdapter = new OleDbDataAdapter(Ole, thisConnection);
            OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);
            DataSet thisDataSet = new DataSet();

            thisAdapter.Fill(thisDataSet, "SubjectFile");

            DataRow thisRow = thisDataSet.Tables["SubjectFile"].NewRow();
            thisRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
            thisRow["SFSUBJDESC"] = DescriptionTextBox.Text;
            thisRow["SFSUBJUNITS"] = UnitsTextBox.Text;
            thisRow["SFSUBJREGOFRNG"] = OfferingComboBox.Text.Substring(0,1);
            thisRow["SFSUBJCATEGORY"] = CategoryComboBox.Text.Substring(0, 1);
            thisRow["SFSUBJSTATUS"] = "AC";
            thisRow["SFSUBJCOURSECODE"] = CourseCodeComboBox.Text;
            thisRow["SFSUBJCURRCODE"] = CurriculumYearTextBox.Text;

            thisDataSet.Tables["SubjectFile"].Rows.Add(thisRow);
            thisAdapter.Update(thisDataSet, "SubjectFile");

            MessageBox.Show("Recorded");
        }

        private void RequisiteTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                thisConnection.Open();
                OleDbCommand thisCommand = thisConnection.CreateCommand();

                string sql = "SELECT * FROM SubjectFile";
                thisCommand.CommandText = sql;

                OleDbDataReader thisDataReader = thisCommand.ExecuteReader();
                
                bool found = false;
                string subjectCode = "";
                string description = "";
                string units = "";

                while (thisDataReader.Read())
                {
                    if (thisDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == RequisiteTextBox.Text.Trim().ToUpper())
                    {
                        found = true;
                        subjectCode = thisDataReader["SFSUBJCODE"].ToString();
                        description = thisDataReader["SFSUBJDESC"].ToString();
                        units = thisDataReader["SFSUBJUNITS"].ToString();
                        break;
                    }
                }

                if (!found)
                    MessageBox.Show("SubjectCode Not Found");
                else
                {
                    SubjectDataGridView.Rows[0].Cells[0].Value = subjectCode;
                    SubjectDataGridView.Rows[0].Cells[1].Value = description;
                    SubjectDataGridView.Rows[0].Cells[2].Value = units;
                }

                //REQUISITE

                OleDbConnection requisiteConnection = new OleDbConnection(connectionString);
                requisiteConnection.Open();
                OleDbCommand requisiteCommand = requisiteConnection.CreateCommand();

                string requisitesql = "SELECT * FROM SubjectPreqFile";
                requisiteCommand.CommandText = requisitesql;

                OleDbDataReader requisiteDataReader = requisiteCommand.ExecuteReader();
                while (requisiteDataReader.Read())
                {
                    if (requisiteDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == RequisiteTextBox.Text.Trim().ToUpper())
                    {
                        SubjectDataGridView.Rows[0].Cells[3].Value = requisiteDataReader["SUBJPRECODE"].ToString().Trim().ToUpper();
                        break;
                    }
                    else
                        SubjectDataGridView.Rows[0].Cells[3].Value = string.Empty;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SubjectScheduleEntry form2 = new SubjectScheduleEntry();
            form2.Show();
            this.Hide();
        }

        private void SubjectDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (SubjectDataGridView.Columns[e.ColumnIndex].CellType == typeof(DataGridViewTextBoxCell) && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void SubjectEntry_Load(object sender, EventArgs e)
        {

        }
    }
}
