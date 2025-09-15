﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChillCofee
{
    public partial class Form1 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Documents\cafe.mdf;Integrated Security=True;Connect Timeout=30");
        public Form1()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void login_registerBtn_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.Show();

            this.Hide();
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        public bool emptyFields()
        {
            if (login_username.Text == "" || login_password.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (emptyFields())
            {
                MessageBox.Show("All fields are required to be filled.", "Error Message", MessageBoxButtons.OK);
            }
            else
            {
                if (connect.State == ConnectionState.Closed)
                {
                    try
                    {
                        connect.Open();

                        string selectAccount = "SELECT COUNT(*) FROM users WHERE username = @usern AND password = @pass AND status = @status";

                        using (SqlCommand cmd = new SqlCommand(selectAccount, connect))
                        {
                            cmd.Parameters.AddWithValue("@usern", login_username.Text.Trim());
                            cmd.Parameters.AddWithValue("@pass", login_password.Text.Trim());
                            cmd.Parameters.AddWithValue("@status", "Active");

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Login successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                AdminMainForm adminForm = new AdminMainForm();
                                adminForm.Show();

                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection failed: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }
    }
}
