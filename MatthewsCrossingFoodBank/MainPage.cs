﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MatthewsCrossingFoodBank
{
    public partial class MainPage : Form
    {
        public static int currentEntry = 0;
        List<string> _list;

        public MainPage()
        {
            InitializeComponent();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.ShowDialog();
            string fileName = fileChooser.FileName;

            txtFilenamePath.Text = fileName;

            List<Donor> donors = getDonors(fileName);

            lblLoadedInfo.Text = donors.Count + "(s) donor entries were loaded.";

            // Populate the data grid view with the data from the CSV file
            foreach (Donor d in donors)
            {
                if (d.firstName == "First Name" && d.lastName == "Last Name") continue;

                String donatedValue;

                if (d.donationType == "Food")
                {
                    donatedValue = d.weight;
                }
                else
                {
                    donatedValue = d.dollarAmount;
                }
                dataGridViewEntries.Rows.Add(d.firstName, d.lastName, d.email, d.donatedOn,
                    d.donationType, donatedValue, d.streetAddress, d.apartment, d.cityTown,
                    d.stateProvince, d.zipPostalCode, d.salutation);
            }
        }

        private void btnDeleteEntry_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridViewEntries.SelectedRows)
            {
                dataGridViewEntries.Rows.RemoveAt(item.Index);
            }
        }

        // Button Process
        private void btnSendEmails_Click(object sender, EventArgs e)
        {
            try
            {
                int totalEntries = dataGridViewEntries.Rows.Count;
                currentEntry = 1;

                progressBarEntries.Visible = true;
                progressBarEntries.Minimum = 1;
                progressBarEntries.Maximum = totalEntries;
                progressBarEntries.Value = 1;
                progressBarEntries.Step = 1;

                // List of HTML strings that do not have email strings and need to be sent letters
                List<string> listHTMLStrings = new List<string>();

                foreach (DataGridViewRow row in dataGridViewEntries.Rows)
                {
                    progressBarEntries.Increment(1);
                    
                    string firstName = "";
                    string lastName = "";
                    string email = "";
                    string date = "";
                    string type = "";
                    string donatedValue = "";
                    string address = "";
                    string apartment = "";
                    string city = "";
                    string state = "";
                    string zipcode = "";
                    string salutation = "";

                    try
                    {
                        if (row.Cells[0].Value != null)
                            firstName = row.Cells[0].Value.ToString();

                        if (row.Cells[1].Value != null)
                            lastName = row.Cells[1].Value.ToString();

                        if (row.Cells[2].Value != null)
                            email = row.Cells[2].Value.ToString();

                        if (row.Cells[3].Value != null)
                            date = row.Cells[3].Value.ToString();

                        if (row.Cells[4].Value != null)
                            type = row.Cells[4].Value.ToString();

                        if (row.Cells[5].Value != null)
                            donatedValue = row.Cells[5].Value.ToString();

                        if (row.Cells[6].Value != null)
                            address = row.Cells[6].Value.ToString();

                        if (row.Cells[7].Value != null)
                            apartment = row.Cells[7].Value.ToString();

                        if (row.Cells[8].Value != null)
                            city = row.Cells[8].Value.ToString();

                        if (row.Cells[9].Value != null)
                            state = row.Cells[9].Value.ToString();

                        if (row.Cells[10].Value != null)
                            zipcode = row.Cells[10].Value.ToString();

                        if (row.Cells[11].Value != null)
                            salutation = row.Cells[11].Value.ToString();
                    }
                    catch (Exception recordException)
                    {
                        Console.WriteLine("Error reading the entries");
                        continue;
                    }

                    if (email == "" && address == "") continue;

                    string stmpServer = "smtp.gmail.com";
                    string fromEmail = "matthewcrossingtest1@gmail.com";
                    string fromPassword = "appleorange123";
                    int port = 587;

                    string subject = "Thank you from Matthew's Crossing Food Bank";

                    string toEmail = email;

                    // Donor has email
                    string htmlString;

                    if (type == "Food")
                    {
                        // Food donation
                        htmlString = getHTMLFoodDonation();

                        int countBlanks = 0;

                        htmlString = htmlString.Replace("«CURRENT_DATE»", DateTime.Now.ToString("MMMM dd, yyyy"));

                        if (firstName == "" && lastName == "")
                        {
                            htmlString = htmlString.Replace("«First_Name»", "");
                            htmlString = htmlString.Replace("«Last_Name»", "");
                            countBlanks++;
                        }
                        else
                        {
                            htmlString = htmlString.Replace("«First_Name»", firstName);
                            htmlString = htmlString.Replace("«Last_Name»", lastName);
                        }


                        htmlString = htmlString.Replace("«Street_Address»", address);
                        if (address == "") countBlanks++;


                        htmlString = htmlString.Replace("«Apartment»", apartment);
                        if (apartment == "") countBlanks++;

                        if (city == "")
                        {
                            // NO city

                            if (state == "")
                            {
                                if (zipcode == "")
                                {
                                    htmlString = htmlString.Replace("«CityTown»", "");
                                    htmlString = htmlString.Replace("«StateProvince»", "");
                                    htmlString = htmlString.Replace("«ZipPostal_Code»", "");
                                    countBlanks++;
                                }
                                else
                                {
                                    htmlString = htmlString.Replace("«CityTown»", "");
                                    htmlString = htmlString.Replace("«StateProvince»", "");
                                    htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                                }
                            }
                            else
                            {
                                htmlString = htmlString.Replace("«CityTown»", "");
                                htmlString = htmlString.Replace("«StateProvince»", state);
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                        }
                        else
                        {
                            // With city

                            if (state != "")
                            {
                                // With state

                                htmlString = htmlString.Replace("«CityTown»", city);
                                htmlString = htmlString.Replace("«StateProvince»", ", " + state);
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                            else
                            {
                                // No state
                                htmlString = htmlString.Replace("«CityTown»", city);
                                htmlString = htmlString.Replace("«StateProvince»", "");
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                        }

                        htmlString = htmlString.Replace("«Salutation_Greeting_Dear_So_and_So»:", salutation);
                        if (salutation == "") countBlanks++;

                        htmlString = htmlString.Replace("«Weight_lbs»", donatedValue);
                        htmlString = htmlString.Replace("«Donated_On»", date);

                        // Add blank lines
                        string replacement = "Executive Director</br>";
                        for (int i = 0; i < countBlanks; i++) replacement += "</br>";
                        htmlString = htmlString.Replace("Executive Director", replacement);
                    }
                    else
                    {
                        // Monetary donation
                        htmlString = getHTMLMonetaryDonation();

                        int countBlanks = 0;


                        htmlString = htmlString.Replace("«CURRENT_DATE»", DateTime.Now.ToString("MMMM dd, yyyy"));

                        if (firstName == "" && lastName == "")
                        {
                            htmlString = htmlString.Replace("«First_Name»", "");
                            htmlString = htmlString.Replace("«Last_Name»", "");
                            countBlanks++;
                        }
                        else
                        {
                            htmlString = htmlString.Replace("«First_Name»", firstName);
                            htmlString = htmlString.Replace("«Last_Name»", lastName);
                        }


                        htmlString = htmlString.Replace("«Street_Address»", address);
                        if (address == "") countBlanks++;


                        htmlString = htmlString.Replace("«Apartment»", apartment);
                        if (apartment == "") countBlanks++;

                        if (city == "")
                        {
                            // NO city

                            if (state == "")
                            {
                                if (zipcode == "")
                                {
                                    htmlString = htmlString.Replace("«CityTown»", "");
                                    htmlString = htmlString.Replace("«StateProvince»", "");
                                    htmlString = htmlString.Replace("«ZipPostal_Code»", "");
                                    countBlanks++;
                                }
                                else
                                {
                                    htmlString = htmlString.Replace("«CityTown»", "");
                                    htmlString = htmlString.Replace("«StateProvince»", "");
                                    htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                                }
                            }
                            else
                            {
                                htmlString = htmlString.Replace("«CityTown»", "");
                                htmlString = htmlString.Replace("«StateProvince»", state);
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                        }
                        else
                        {
                            // With city

                            if (state != "")
                            {
                                // With state

                                htmlString = htmlString.Replace("«CityTown»", city);
                                htmlString = htmlString.Replace("«StateProvince»", ", " + state);
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                            else
                            {
                                // No state
                                htmlString = htmlString.Replace("«CityTown»", city);
                                htmlString = htmlString.Replace("«StateProvince»", "");
                                htmlString = htmlString.Replace("«ZipPostal_Code»", zipcode);
                            }
                        }

                        htmlString = htmlString.Replace("«Salutation_Greeting_Dear_So_and_So»:", salutation);
                        if (salutation == "") countBlanks++;

                        htmlString = htmlString.Replace("«M_Amount»", donatedValue);
                        htmlString = htmlString.Replace("«Donated_On»", date);

                        // Add blank lines
                        string replacement = "Executive Director</br>";
                        for (int i = 0; i < countBlanks; i++) replacement += "</br>";
                        htmlString = htmlString.Replace("Executive Director", replacement);
                    }


                    if (email != "")
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient(stmpServer);

                        mail.From = new MailAddress(fromEmail);
                        mail.To.Add(toEmail);
                        mail.Subject = subject;
                        SmtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                        mail.Body = htmlString;
                        mail.IsBodyHtml = true;

                        SmtpServer.Port = port;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword);
                        SmtpServer.EnableSsl = true;

                        SmtpServer.SendMailAsync(mail);
                    }
                    else
                    {
                        // Only create HTML document for valid addresses
                        if (address == "" || zipcode == "" || state == "" || city == "") continue;

                        listHTMLStrings.Add(htmlString);
                    }
                }

                // Call method to transform html strings into document
                createHTML(listHTMLStrings);


                MessageBox.Show("Messages sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Unable to send email to " + ex.Message);

                Console.WriteLine("Exception Message: " + ex.Message + "\n\n");
                Console.WriteLine("Exception String: " + ex.ToString());
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Send canceled.");
            }
            if (e.Error != null)
            {
                Console.WriteLine("Error " + e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }

            currentEntry++;
        }

        public bool containsKeyword(string word)
        {
            string lower = word.ToLower();
            // Filter out companies
            string[] keywords = txtKeywords.Text.ToString().Split(',');

            for (int i = 0; i < keywords.Length; i++)
            {
                if (lower.Contains(keywords[i].ToLower()))
                    return true;
            }

            return false;
        }

        public List<Donor> getDonors(String filename)
        {
            string path = @"" + filename;
            string line;
            string[] parsedLine;

            List<Donor> donorList = new List<Donor>();

            StreamReader sr = new StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                parsedLine = SplitCSV(line);

                if (containsKeyword(parsedLine[2]) || containsKeyword(parsedLine[3]))
                {
                    continue;
                }

                if (parsedLine[1] != "1")
                {
                    donorList.Add(addDonor(parsedLine));
                }
            }

            sr.Close();

            return donorList;
        }

        public static Donor addDonor(string[] donorArray)
        {
            Donor temp = new Donor();

            temp.isCompany = donorArray[1].Replace("\"", "");
            temp.firstName = donorArray[2].Replace("\"", "");
            temp.lastName = donorArray[3].Replace("\"", "");
            temp.email = donorArray[4].Replace("\"", "");
            temp.salutation = donorArray[5].Replace("\"", "");
            temp.streetAddress = donorArray[6].Replace("\"", "");
            temp.apartment = donorArray[7].Replace("\"", "");
            temp.cityTown = donorArray[8].Replace("\"", "");
            temp.stateProvince = donorArray[9].Replace("\"", "");
            temp.zipPostalCode = donorArray[10].Replace("\"", "");
            temp.donationType = donorArray[11].Replace("\"", "");
            temp.donatedOn = donorArray[12].Replace("\"", "");
            temp.dollarAmount = donorArray[13].Replace("\"", "");
            temp.weight = donorArray[14].Replace("\"", "");

            return temp;
        }

        public static string[] SplitCSV(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }
                list.Add(curr.TrimStart(','));
            }
            return list.ToArray<string>();
        }

        public static string getHTMLFoodDonation()
        {
            return @"<div id="":12n"" class=""Ar Au"" style=""display: block; position: relative;""><div class=""WS_li_overlay"" style=""display: none; border-radius: 3px; position: absolute; z-index: 14; height: 18px; background: none; top: 142px; left: 7px; width: 169px;""><div class=""WS_overlay_button"" style=""cursor: pointer; border-radius: 3px; font-size: 12px; float: left; padding: 5px 2px; margin: 0px; background-color: rgb(179, 186, 45); color: white; position: relative; height: 15px; width: 167px;""><div style=""display: table; margin: -1px auto;"">Remove this branding</div><img style=""position: absolute; display: table; width: 9px; height: 9px; top: 8px; right: 5px;"" src=""chrome-extension://pbcgnkmbeodkmiijjfnliicelkjfcldg/content/img/x-close-hover.png""></div></div><img class=""WS_x_i"" style=""position: absolute; cursor: pointer; z-index: 13; display: block; width: 9px; height: 9px; left: 164px; top: 150px;"" src=""chrome-extension://pbcgnkmbeodkmiijjfnliicelkjfcldg/content/img/x-close.png""><div id="":12r"" class=""Am Al editable LW-avf"" hidefocus=""true"" aria-label=""Message Body"" g_editable=""true"" role=""textbox"" contenteditable=""true"" tabindex=""1"" style=""direction: ltr; min-height: 268px;"" data-wisestamp-editor-id=""736877222638713"" wisestamped=""true"" wsmode=""compose"" data-space-before-signature=""0""><div id=""gmail-WISESTAMP_SIG_7ed41a7ab964e60db71200cde2ac8670"" title=""WISESTAMP_SIG_7ed41a7ab964e60db71200cde2ac8670""><span id=""gmail-docs-internal-guid-96df4bce-42bf-cc1b-a18b-d0feecfe435a""><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""><img src=""https://lh4.googleusercontent.com/04qJQBWmXUuMHsPIOUemkCq-8W4k_nLEqUidZRNSizR1zNm_8aji0rrj3sj6gZUyqKdfdYvRYvVoHoW9NjuP2Tg-RYH6o3H0nnNvqcTtgNv2PzI67cNXBXPF-axBK0srvkNogKsFauaJOg_XRg"" width=""372"" height=""88"" style=""border: none; transform: rotate(0rad);""></span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«CURRENT_DATE»</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«First_Name» «Last_Name»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«Street_Address»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«Apartment»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«CityTown» «StateProvince» &nbsp; «ZipPostal_Code»</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-weight: 700; vertical-align: baseline; white-space: pre-wrap;"">«Salutation_Greeting_Dear_So_and_So»:</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-weight: 700; vertical-align: baseline; white-space: pre-wrap;"">WE JUST WANT TO THANK YOU.</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Thank you for showing your commitment to fighting hunger in our community by your generous food donation of «Weight_lbs» lbs. on «Donated_On».</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We at Matthew’s Crossing continue to provide food assistance to individuals and families in need, specifically the working poor, children, seniors and individuals with disabilities on a fixed income, families in crisis and the homeless. &nbsp; </span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We are seeing record numbers of new clients needing our help. &nbsp; We couldn’t continue providing these critical services without our amazing supporters, and we are especially grateful to repeat donors like you.</span></p><br><ul style=""margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew's Crossing is a 501 (C) (3) tax-exempt organization (EIN 55-0896414) under the provisions of the Internal Revenue Code. &nbsp;</span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We acknowledge that your gift was received in our office and credited as a tax-deductible contribution for the calendar year 2017. </span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">No goods or services were received in exchange for this donation, other than joy of giving to an organization that is helping to fight hunger in our community. </span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew's Crossing is included on the approved list of qualified charitable organizations published by the Arizona Department of Revenue and this contribution qualified for a charitable Arizona tax credit for the working poor. &nbsp; The allowable tax credit is up to $400 for single and up to $800 for joint tax returns. &nbsp; Please consult the Arizona Department of Revenue at </span><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 255); background-color: transparent; text-decoration-line: underline; vertical-align: baseline; white-space: pre-wrap;"">www.azdor.gov</span><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""> for more information</span></p></li></ul><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Please call if you have any questions. &nbsp; I can’t express enough how much we appreciate your support.</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Sincerely, </span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""><img src=""https://lh6.googleusercontent.com/xYvCMnR0UzDRL8Xzov_93m2VipBX4B5Kf1lbc_UWdVgBus3i7W9WGdmsJeofUzEl-BEMA8I96FXnK9bk40k5BKqHJvPAcfN45uqYASg6ljAEU5ogzbqL9i0r1hAszgGRRVJCRs4MaDe1uRm7kg"" width=""135"" height=""65"" style=""border: none; transform: rotate(0rad);"" alt=""C:\Users\Business Manager\AppData\Local\Microsoft\Windows\INetCache\Content.Word\Signature.jpg""></span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Jan Terhune</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Executive Director</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center; border-top: 0.5pt solid rgb(0, 0, 0); padding: 4pt 0pt 0pt;""><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew’s Crossing Food Bank ● 1368 N Arizona Ave, Suite 112 Chandler, AZ 85225 ● (480) 857-2296 ● www.matthewscrossing.org</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Compassion</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 176, 80); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">●</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Dignity</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 176, 80); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">●</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Hope</span></p><div><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""><br></span></div></span></div></div></div>";
        }

        public static string getHTMLMonetaryDonation()
        {
            return @" <div id=""gmail-WISESTAMP_SIG_7ed41a7ab964e60db71200cde2ac8670"" title=""WISESTAMP_SIG_7ed41a7ab964e60db71200cde2ac8670""><span id=""gmail-docs-internal-guid-28b85b31-41d3-a101-d119-59a1c0f4d41d""><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""><img src=""https://lh3.googleusercontent.com/jsqClq4FaMGDXuVBAEWLH4WaJZvnzm-sjgF3BepNccxVKF5Q6fsDFoo0BtpOl5RC9qbreNRhZYUGdy6c8-dkoApIksC1lwAmZ6PTTPadO807b-JGRD6FIPXFso6mbuM9I3wnQZIKACLCel9i-g"" width=""372"" height=""88"" style=""border: none; transform: rotate(0rad);""></span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«CURRENT_DATE»</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«First_Name» «Last_Name»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«Street_Address»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«Apartment»</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">«CityTown» «StateProvince» &nbsp;«ZipPostal_Code»</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-weight: 700; vertical-align: baseline; white-space: pre-wrap;"">«Salutation_Greeting_Dear_So_and_So»:</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-weight: 700; vertical-align: baseline; white-space: pre-wrap;"">WE JUST WANT TO THANK YOU.</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Thank you for showing your commitment to fighting hunger in our community by sending your generous gift of $«M_Amount» received on «Donated_On».</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We are absolutely thrilled that you chose to support Matthew’s Crossing. &nbsp;Because of your gift families will be able to feed their children. &nbsp;</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We at Matthew’s Crossing continue to provide food assistance to individuals and families in need, specifically the working poor, children, seniors and individuals with disabilities on a fixed income, families in crisis and the homeless. </span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We are seeing record numbers of new clients needing our help and truly, we couldn't continue to provide our critical services without your support.</span></p><br><ul style=""margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew's Crossing is a 501 (C) (3) tax-exempt organization (EIN 55-0896414) under the provisions of the Internal Revenue Code. &nbsp;</span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">We acknowledge that your gift was received in our office and credited as a tax-deductible contribution for the calendar year 2017. </span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">No goods or services were received in exchange for this donation, other than joy of giving to an organization that is helping to fight hunger in our community. </span></p></li><li dir=""ltr"" style=""list-style-type: disc; font-size: 11pt; font-family: &quot;Noto Sans Symbols&quot;; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline;""><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt;""><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew's Crossing is included on the approved list of qualified charitable organizations published by the Arizona Department of Revenue and this contribution qualified for a charitable Arizona tax credit for the working poor. &nbsp;The allowable tax credit is up to $400 for single and up to $800 for joint tax returns. &nbsp;Please consult the Arizona Department of Revenue at </span><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 255); background-color: transparent; text-decoration-line: underline; vertical-align: baseline; white-space: pre-wrap;"">www.azdor.gov</span><span style=""font-size: 11pt; font-family: Calibri; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""> for more information.</span></p></li></ul><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Please call if you have any questions. &nbsp;I can’t express enough how much we appreciate your support.</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Sincerely,</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;""><img src=""https://lh5.googleusercontent.com/LFJKF5Sb-bLR30GSioqyrlnXHZKE1w6hMiwrZrtETnjWato2lVHtaWyiQYaX4gx2-4m_TybfFrimGFtKMEl64H3UOOPsRDmW3arkrCzsqywOKGLoqN1SJUFp-9HdTO6cNadsx2aUz7ZwiqC-mA"" width=""118"" height=""57"" style=""border: none; transform: rotate(0rad);"" alt=""C:\Users\Business Manager\AppData\Local\Microsoft\Windows\INetCache\Content.Word\Signature.jpg""></span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Jan Terhune</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: justify;""><span style=""font-size: 11pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Executive Director</span></p><br><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center; border-top: 0.5pt solid rgb(0, 0, 0); padding: 4pt 0pt 0pt;""><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; vertical-align: baseline; white-space: pre-wrap;"">Matthew’s Crossing Food Bank ● 1368 N Arizona Ave Suite 112, Chandler, AZ 85225 ● (480) 857-2296 ● matthewscrossing.org</span></p><p dir=""ltr"" style=""line-height: 1.2; margin-top: 0pt; margin-bottom: 0p; margin-top: 0pt; margin-bottom: 0pt; margin-left: 25pt; margin-right: 25pt; text-align: center;""><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Compassion</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 176, 80); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">●</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Dignity</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 176, 80); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">●</span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(0, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""> </span><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;"">Hope</span></p><div><span style=""font-size: 8pt; font-family: Calibri; color: rgb(204, 0, 0); background-color: transparent; font-style: italic; vertical-align: baseline; white-space: pre-wrap;""><br></span></div></span></div>";
        }

        private void createHTML(List<string> letters)
        {
            if (letters.Count <= 0) return;

            //create new SaveFileDialog instance
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "html files (*.html)|*.html|All files (*.*)|*.*";

            //display dialog and see if OK button was pressed
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = File.CreateText(sfd.FileName))
                {
                    foreach (String str in letters)
                        sw.Write(str);
                }

            }

            SendToPrinter(sfd.FileName);
        }

    private void SendToPrinter(string filename)
        {
            string filePath = filename;

            Process print = new Process();

            print.StartInfo.FileName = filePath;

            print.StartInfo.Verb = "";

            print.StartInfo.UseShellExecute = true;

            print.StartInfo.CreateNoWindow = true;

            print.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            print.Start();

            print.CloseMainWindow();

            print.Close();
        }

        private void dataGridViewEntries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void btnDataAnalytics_Click(object sender, EventArgs e)
        {
            string filename = txtFilenamePath.Text;

            List<Donor> donors = getDonors(filename);

            DataAnalytics dataAnalytics = new DataAnalytics(donors);
            dataAnalytics.ShowDialog();
        }
    }
}
