using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;    
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Tools;


namespace VendorCallListUpdater
{

	public partial class Form1 : Form
	{
		// VAR DECLARES
		private const string userID = "i3cic";
		private const string passwd = "";
		private string table = "";
		private const string server = "ksms1481";
		private string databaseTable= "Collections";
		private const string backupDestination = @"\\" + server + @"\incoming";
		private static SqlConnection db;
		// END VAR DECLARES
		public Form1()
		{
			InitializeComponent();
		}
		private void Append_Button_Click(object sender, EventArgs e)
		{
			// Select File to open
			try
			{
				openFile.ShowDialog();
			}
			catch (Exception a)
			{
				System.Diagnostics.Debug.WriteLine(a.ToString());
			}
			
			// Backup
			backup();
			// Confirmation
			if (MessageBox.Show("Really append files?", "Confirm append", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			// Set up updateList
			List<string[]> updateList = new List<string[]>();
			updateList = parseCSV(openFile.FileName);    
			// remove header row
			updateList.RemoveAt(0);
			// set up progress bar
			progressBar.Maximum = updateList.Count;
			// Loop through updateList
			int count = 0;
			foreach (string[] aRow in updateList)
			{
				// build sql
				string tmp = buildSQL(aRow);
				// Run sql
				sqlInsert(tmp);
				// increment progress bar
				progressBar.Value++;
				count++;
			}
			// Run fixup step
			fixupStep();
			// Output job well done
			MessageBox.Show("Lists appended.  ");
			// reset progress bar
			progressBar.Value = 0;
			// enable undo
			undoButton.Enabled = true;
		}    
		private void sqlInsert(string sqlToInsert)
		{
			// build SqlCommand
			SqlCommand sql = new SqlCommand(sqlToInsert, db);
			if (db.State.Equals("Closed"))
				openDbConnection();

			// run sql
			try
			{
				sql.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				String str = "Error for insertion " + ex.Message + Environment.NewLine;
				str += "StackTrace : " + ex.StackTrace + Environment.NewLine;
				if ( ex.InnerException != null )
				{
					str += "Inner1 : " + ex.InnerException.Message + Environment.NewLine;
					if ( ex.InnerException.InnerException != null )
					{
						str += "Inner2 : " + ex.InnerException.InnerException.Message + Environment.NewLine;
					}
				}
				str += "\n\nSQL : " + sqlToInsert;
				MessageBox.Show(str);
				//or
				Console.WriteLine(str);
			}

		 
			 System.Diagnostics.Debug.WriteLine("SQL Inserted");
		}
		private List<string[]> parseCSV(string path)
		{
			List<string[]> parsedData = new List<string[]>();
			try
			{
				using (StreamReader readFile = new StreamReader(path))
				{
					string line;
					string[] row;
					while ((line = readFile.ReadLine()) != null)
					{
						// Check for Quotes
						if (line.Contains("\""))
							{
							int start = line.IndexOf("\"");
							line = line.Remove(line.IndexOf(",", start), 1);
						   /* string tmp = line.Substring((line.IndexOf("\"")),line.LastIndexOf("\""));
							MessageBox.Show(tmp);
							line = line.Replace((char)34, '\'');
							MessageBox.Show(line);*/
							}
						row = line.Split(',');
						parsedData.Add(row);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return parsedData;
		}
		private string buildSQL(string[] inputLine)
		{
			// Fix for comma at end
			// Build valueList 
			// String fixes
			// trim down name 
			if (inputLine[1].Length > 25)
				inputLine[1] = inputLine[1].Substring(0, 25);
			// trim down address
			if (inputLine[2].Length > 25)
				inputLine[2] = inputLine[2].Substring(0, 25);
			// trim down city
			if (inputLine[3].Length > 30)
				inputLine[3] = inputLine[3].Substring(0, 30);
			// trim down state
			if (inputLine[4].Length > 2)
				inputLine[4] = inputLine[4].Substring(0, 2);
			// trim down zip
			if (inputLine[5].Length > 5)
				inputLine[5] = inputLine[5].Substring(0, 5);
			// trim down Phones 
			for(int f=6;f<13;f++)
				if (inputLine[f].Length > 12)
					inputLine[f] = inputLine[f].Substring(0, 12);
			// Handle blank phone1's
			if (inputLine[6].Equals(""))
			    inputLine[6] = "5555555555";
			// trim down dunninglevel 
			if (inputLine[14].Length > 1)
				inputLine[14] = inputLine[14].Substring(0, 5);
			// trim down cyclenumber 
			if (inputLine[15].Length > 2)
				inputLine[15] = inputLine[15].Substring(0, 2);
			// trim down acctbalance 
			if (inputLine[17].Length > 2)
				inputLine[16] = inputLine[16].Trim();
			// loop through each entry in inputLine[] and add it to valueString
			string valueString = "";
			for (int i = 0; i <= 17; i++)
			{
				// Check for any apostrophes
				inputLine[i] = inputLine[i].Replace("'", "''");
				valueString += ", ";
				valueString += "'" + inputLine[i] + "'";
				
			}
			// trim leading ','
			valueString = valueString.TrimStart(',');
			// trim trailing ','
			//valueString = valueString.Remove(valueString.Length - 4, 4);
			String sql = "INSERT INTO " + table + "(i3_rowid, Name, Address, City, State, Zip, Phone1, Phone2, Phone3, Phone4, Phone5, Phone6, Phone7, Phone8, score, cycleNumber, delinquentDays, acctbalance) ";
			sql += " VALUES (" + valueString + ")";
			//System.Diagnostics.Debug.WriteLine(sql);
			return sql;
		}
		private SqlDataReader sqlSelect(string sqlToSelect)
		{
			// build SqlCommand
			SqlCommand sql = new SqlCommand(sqlToSelect, db);
			// Run sql
			try
			{
				SqlDataReader reader = sql.ExecuteReader();
				return reader;
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("ERROR");
			}

			return null;
		}
		private void Load_Button_Click(object sender, EventArgs e)
		{
		// Select File to open
			try
			{
				openFile.ShowDialog();
			}
			catch (Exception a)
			{
				System.Diagnostics.Debug.WriteLine(a.ToString());
			}  
			// Backup DB
			backup();
			// Confirmation
			if (MessageBox.Show("Really Load Files?", "Confirm load", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			// delete old table
			sqlInsert("DELETE FROM " + table);
			// Set up updateList
			List<string[]> updateList = new List<string[]>();
			updateList = parseCSV(openFile.FileName);    
			// remove header row
			updateList.RemoveAt(0);
			// set up progress bar
			progressBar.Maximum = updateList.Count;
			// Loop through updateList
			foreach (string[] aRow in updateList)
			{
				// build sql
				string tmp = buildSQL(aRow);
				// Run sql
				sqlInsert(tmp);
				// increment progress bar
				progressBar.Value++;
			}
			// Run fixup step
			fixupStep();
			// Output job well done
			MessageBox.Show("Lists loaded.");
			// reset progress bar
			progressBar.Value = 0;
			// enable undo
			undoButton.Enabled = true;
		}
		private void backup()
		{
			// Execute Select * statement
			string sql = "SELECT * FROM " + table;
			SqlDataReader toSave = sqlSelect(sql);
			// Save read data to .bak file
			string tmp = "";
			string tmpSave = "";
			List<string> lines = new List<string>();
			// Handle null db
			if (toSave != null)
				{
				while (toSave.Read())
					{
					for (int i = 0; i < toSave.FieldCount; i++)
						{
						//Console.WriteLine(toSave[i]);
						tmpSave = toSave[i].ToString().Replace("'", "''");
						tmp += "'" + tmpSave + "'" + ", ";
						}
					lines.Add(tmp);
					// reset tmp
					tmp = "";
					}
				// close the SqlDataReader
				toSave.Close();
				}
			// Delete existing backup file
			System.IO.File.Delete(backupDestination + @"\dbBackup.txt");
			// Write to backup file
			System.IO.File.WriteAllLines(backupDestination + @"\dbBackup.txt", lines.ToArray());

		}
		private void restore()
		{
			// Confirmation
			if (MessageBox.Show("Really Undo Change?", "Confirm undo", MessageBoxButtons.YesNo) == DialogResult.No)
				{
				return;
				}
			// Clear out table
			sqlInsert("DELETE FROM " + table);
			// Set up vars for insertion
			string line;
			int counter = 0;
			// Open backup file for reading
			System.IO.StreamReader file = new System.IO.StreamReader(backupDestination + @"\dbBackup.txt");
			while ((line = file.ReadLine()) != null)
			{
				// Remove trailing comma space
				line = line.Remove(line.Length - 28, 28);
				// Delete identity key data
				// run sql
				sqlInsert("INSERT INTO " + table + " (I3_RowId, Name, Address, City, State, Zip, Zone, SSN, Status, Attempts, Phone1, Phone2, Phone3, Phone4, Phone5, Phone6, Phone7, Phone8, DunningLevel, CycleNumber, Score,AcctBalance, i3_attemptsremotehangup, i3_attemptssystemhangup, i3_attemptsabandoned, i3_attemptsbusy, i3_attemptsfax, i3_attemptsnoanswer, i3_attemptsmachine, i3_attemptsrescheduled, moneycounter, i3_siteid, i3_activeworkflowid)" + " VALUES (" +line + ")");
				// increment counter
				counter++;
			}
			file.Close();
			File.Delete(backupDestination + @"\dbBackup.txt");
			Console.WriteLine(counter + " Lines read.");
			// check number of lines before and after

		   
			// output to messagebox.show()
			MessageBox.Show("Backup restored! " + counter + " records modified");
			// disable undo button
			undoButton.Enabled = false;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			// Undo
			restore();
		}
		private void tablePicker_SelectedIndexChanged(object sender, EventArgs e)
			{
			table = Convert.ToString(tablePicker.SelectedValue);
			// Exception for SkipCallList
			if (table.Equals("VendorSkipCallList"))
				databaseTable = "CreditRecovery";
			else
				databaseTable = "Collections";

			// Reset db connection
			openDbConnection();
			}
		private void Form1_Load(object sender, EventArgs e)
			{
			string line;
			ArrayList tableList = new ArrayList();
			System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tableList.ini");
			while ((line = file.ReadLine()) != null)
				tableList.Add(line);
			tablePicker.DataSource = tableList;
			// Open DB Connection
			try
				{
				using (new Impersonator(userID, "cp", passwd))
					{

					db = new SqlConnection(
						"user id=" + userID + ";" +
						"password=" + passwd + ";" +
						"server=" + server + ";" +
						"Trusted_Connection=yes;" +
						"database=" + databaseTable +
						";connection timeout=30"
						);
					db.Open();
					}
				}
			catch (Exception a)
				{
				System.Diagnostics.Debug.WriteLine(a.ToString());
				}
			}
		private void fixupStep()
			{
			// Set fixup step script name
			string name = "";
			switch (tablePicker.Text)
				{
				case "VendorCallList": name = "Common.dbo.FixUpVendorCallList 'Collections'";
					break;
				case "VendorAccurintCallList": name = "Common.dbo.FixUpVendorAccurintCallList2 'Collections'";
					break;
				case "VendorInnovisCallList": name = "Common.dbo.FixUpVendorInnovisCallList 'Collections'";
					break;
				case "testJanesVendorCallList":
					goto theEnd;
				case "VendorSkipCallList": name = "Common.dbo.FixUpVendorSkipCallListFD 'CreditRecovery'";
					break;

				}

			// build sql
			SqlCommand sql = new SqlCommand("execute " + name + " ", db);
			// execute
			try
				{
				sql.ExecuteNonQuery();
				}
			catch (Exception ex)
				{
				Console.WriteLine("ERROR WITH FIXUP STEP");
				String str = "Error for insertion " + ex.Message + Environment.NewLine;
				str += "StackTrace : " + ex.StackTrace + Environment.NewLine;
				if (ex.InnerException != null)
					{
					str += "Inner1 : " + ex.InnerException.Message + Environment.NewLine;
					if (ex.InnerException.InnerException != null)
						{
						str += "Inner2 : " + ex.InnerException.InnerException.Message + Environment.NewLine;
						}
					}
				MessageBox.Show(str);
				Console.WriteLine(str);
				}
		theEnd:
			Console.WriteLine("End Fixup Step");
			}
		private void openDbConnection()
			{
			// Open DB Connection
			try
				{
				using (new Impersonator(userID, "cp", passwd))
					{

					db = new SqlConnection(
						"user id=" + userID + ";" +
						"password=" + passwd + ";" +
						"server=" + server + ";" +
						"Trusted_Connection=yes;" +
						"database=" + databaseTable +
						";connection timeout=30"
						);
					db.Open();
                    // Set ANSI_NULLS and ANSI_WARNINGS
                    sqlInsert("SET ANSI_NULLS ON");
					}
				}
			catch (Exception a)
				{
				System.Diagnostics.Debug.WriteLine(a.ToString());
				}
			}
  }
}
