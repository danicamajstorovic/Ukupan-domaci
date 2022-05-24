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
    public partial class Osoba : Form
    {
        int broj_sloga = 0;
        DataTable tabela;

        public Osoba()
        {
            InitializeComponent();
        }

        private void Load_Data()
        {
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Osoba", veza);
            tabela = new DataTable();
            adapter.Fill(tabela);
        }

        private void Txt_Load()
        {
            if (tabela.Rows.Count != 0)
            {
                txt_id.Text = tabela.Rows[broj_sloga]["id"].ToString();
                txt_ime.Text = tabela.Rows[broj_sloga]["ime"].ToString();
                txt_prezime.Text = tabela.Rows[broj_sloga]["prezime"].ToString();
                txt_adresa.Text = tabela.Rows[broj_sloga]["adresa"].ToString();
                txt_jmbg.Text = tabela.Rows[broj_sloga]["jmbg"].ToString();
                txt_email.Text = tabela.Rows[broj_sloga]["email"].ToString();
                txt_pass.Text = tabela.Rows[broj_sloga]["pass"].ToString();
                txt_uloga.Text = tabela.Rows[broj_sloga]["uloga"].ToString();


                btn_delete.Enabled = true;
                if (broj_sloga == 0)
                {
                    btn_first.Enabled = false;
                    btn_prev.Enabled = false;
                }
                else
                {
                    btn_first.Enabled = true;
                    btn_prev.Enabled = true;
                }

                if (broj_sloga == tabela.Rows.Count - 1)
                {
                    btn_last.Enabled = false;
                    btn_next.Enabled = false;
                }
                else
                {
                    btn_last.Enabled = true;
                    btn_next.Enabled = true;
                }
            }
            else
            {
                txt_id.Text = "";
                txt_ime.Text = "";
                txt_prezime.Text = "";
                txt_adresa.Text = "";
                txt_jmbg.Text = "";
                txt_email.Text = "";
                txt_pass.Text = "";
                txt_uloga.Text = "";

                btn_first.Enabled = false;
                btn_prev.Enabled = false;
                btn_last.Enabled = false;
                btn_next.Enabled = false;
                btn_update.Enabled = false;
                btn_delete.Enabled = false;
            }
        }

        private void Osoba_Load(object sender, EventArgs e)
        {
            Load_Data();
            Txt_Load();
        }

        private void btn_first_Click(object sender, EventArgs e)
        {
            broj_sloga = 0;
            Txt_Load();
        }

        private void btn_last_Click(object sender, EventArgs e)
        {
            broj_sloga = tabela.Rows.Count - 1;
            Txt_Load();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            broj_sloga--;
            Txt_Load();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            broj_sloga++;
            Txt_Load();
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            /*INSERT INTO Osoba(ime, prezime, adresa, jmbg, email, pass, uloga)
            VALUES('Marko', 'Lazic', 'Savska 10', '1234564312341', 'markol@gmail.com',
            '1234', 'ucenik')*/
            StringBuilder naredba = new StringBuilder("INSERT INTO Osoba(ime, prezime, adresa, jmbg, email, pass, uloga) VALUES ('");
            naredba.Append(txt_ime.Text + "', '");
            naredba.Append(txt_prezime.Text + "', '");
            naredba.Append(txt_adresa.Text + "', '");
            naredba.Append(txt_jmbg.Text + "', '");
            naredba.Append(txt_email.Text + "', '");
            naredba.Append(txt_pass.Text + "', '");
            naredba.Append(txt_uloga.Text + "')");
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza);

            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }

            Load_Data();
            broj_sloga = tabela.Rows.Count - 1;
            Txt_Load();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            /*UPDATE Osoba SET ime = 'Marko', prezime = 'Peric',
            adresa = 'Studenac 8', jmbg = '1234567891234', 
            email = 'MarkoP@yahoo.com', pass = '1222', uloga = 'ucenik'
            WHERE id = 3*/
            StringBuilder naredba = new StringBuilder("UPDATE Osoba SET ");
            naredba.Append("ime = '" + txt_ime.Text + "', ");
            naredba.Append("prezime = '" + txt_prezime.Text + "', ");
            naredba.Append("adresa = '" + txt_adresa.Text + "', ");
            naredba.Append("jmbg = '" + txt_jmbg.Text + "', ");
            naredba.Append("email = '" + txt_email.Text + "', ");
            naredba.Append("pass = '" + txt_pass.Text + "', ");
            naredba.Append("uloga = '" + txt_uloga.Text + "' ");
            naredba.Append("WHERE id = " + txt_id.Text);
            //MessageBox.Show(naredba.ToString());
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza);

            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }

            Load_Data();
            Txt_Load();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string naredba = "DELETE FROM Osoba WHERE id = " + txt_id.Text;
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            Boolean brisano = false;
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
                brisano = true;
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }

            if (brisano)
            {
                Load_Data();
                if (broj_sloga > 0) broj_sloga--;

                Txt_Load();
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            Load_Data();
            Txt_Load();
        }

        private void lbl_ime_Click(object sender, EventArgs e)
        {

        }
    }
}
