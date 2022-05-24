using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Ednevnik
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_name.Text == "" || txt_passw.Text == "")
            {
                MessageBox.Show("Molimo unesite sve podatke!");
                return;
            }
            else
            {

                try
                {
                    SqlConnection veza = Konekcija.Connect();
                    SqlCommand komanda = new SqlCommand("SELECT * FROM osoba where email =@username", veza);
                    komanda.Parameters.AddWithValue("@username", txt_name.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    int brojac = tabela.Rows.Count;
                    if (brojac == 1)
                    {
                        if (string.Compare(tabela.Rows[0]["pass"].ToString(), txt_passw.Text) == 0)
                        {
                            MessageBox.Show("Uspesna prijava!");
                            Program.user_ime = tabela.Rows[0]["ime"].ToString();
                            Program.user_prezime = tabela.Rows[0]["prezime"].ToString();
                            Program.user_uloga = (int)tabela.Rows[0]["uloga"];
                            this.Hide();
                            //Glavna frm_glavna = new Glavna();
                            //frm_glavna.Show();
                            Glavna2 frm_glavna2 = new Glavna2();
                            frm_glavna2.Show();
                        }
                        else
                        {
                            MessageBox.Show("Los password!");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Nepostojeca email adresa!");
                    }

                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                }
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

    }
}
