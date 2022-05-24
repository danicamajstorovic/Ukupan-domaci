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
    public partial class Raspodela : Form
    {
        DataTable raspodela;
        int broj_sloga = 0;
        public Raspodela()
        {
            InitializeComponent();
        }
        private void Load_data()
        {
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM raspodela", veza);
            raspodela = new DataTable();
            adapter.Fill(raspodela);
        }
        private void Raspodela_Load(object sender, EventArgs e)
        {
            Load_data();
            ComboFill();
        }
        private void ComboFill()
        {
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter;
            DataTable dt_godina, dt_nastavnik, dt_predmet, dt_odeljenje;

            adapter = new SqlDataAdapter("SELECT * FROM skolska_godina", veza);
            dt_godina = new DataTable();
            adapter.Fill(dt_godina);

            adapter = new SqlDataAdapter("SELECT id, ime + ' ' + prezime AS naziv FROM Osoba WHERE uloga = 2", veza);
            dt_nastavnik = new DataTable();
            adapter.Fill(dt_nastavnik);

            adapter = new SqlDataAdapter("SELECT id, naziv FROM Predmet", veza);
            dt_predmet = new DataTable();
            adapter.Fill(dt_predmet);

            adapter = new SqlDataAdapter("SELECT id, STR(razred) + '-' + indeks AS naziv FROM Odeljenje", veza);
            dt_odeljenje = new DataTable();
            adapter.Fill(dt_odeljenje);

            cmb_godina.DataSource = dt_godina;
            cmb_godina.ValueMember = "id";
            cmb_godina.DisplayMember = "naziv";

            cmb_nastavnik.DataSource = dt_nastavnik;
            cmb_nastavnik.ValueMember = "id";
            cmb_nastavnik.DisplayMember = "naziv";

            cmb_predmet.DataSource = dt_predmet;
            cmb_predmet.ValueMember = "id";
            cmb_predmet.DisplayMember = "naziv";

            cmb_odeljenje.DataSource = dt_odeljenje;
            cmb_odeljenje.ValueMember = "id";
            cmb_odeljenje.DisplayMember = "naziv";

            txt_id.Text = raspodela.Rows[broj_sloga]["id"].ToString();

            if (raspodela.Rows.Count == 0)
            {
                cmb_godina.SelectedValue = -1;
                cmb_nastavnik.SelectedValue = -1;
                cmb_predmet.SelectedValue = -1;
                cmb_odeljenje.SelectedValue = -1;
            }
            else
            {
                cmb_godina.SelectedValue = raspodela.Rows[broj_sloga]["godina_id"];
                cmb_nastavnik.SelectedValue = raspodela.Rows[broj_sloga]["nastavnik_id"];
                cmb_predmet.SelectedValue = raspodela.Rows[broj_sloga]["predmet_id"];
                cmb_odeljenje.SelectedValue = raspodela.Rows[broj_sloga]["odeljenje_id"];
            }
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
            if (broj_sloga == raspodela.Rows.Count - 1)
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

        private void btn_first_Click(object sender, EventArgs e)
        {
            broj_sloga = 0;
            ComboFill();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            broj_sloga--;
            ComboFill();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            broj_sloga++;
            ComboFill();
        }

        private void btn_last_Click(object sender, EventArgs e)
        {
            broj_sloga = raspodela.Rows.Count - 1;
            ComboFill();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string naredba = "DELETE FROM Raspodela WHERE id = " + txt_id.Text;
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
                Load_data();
                if (broj_sloga > 0) broj_sloga--;
                ComboFill();
            }
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            /*INSERT INTO Raspodela (godina_id, nastavnik_id, predmet_id, odeljenje_id)
            VALUES(7, 2, 2, 1)*/
            StringBuilder naredba = new StringBuilder("INSERT INTO Raspodela (godina_id, nastavnik_id, predmet_id, odeljenje_id) VALUES ('");
            naredba.Append(cmb_godina.SelectedValue + "', '");
            naredba.Append(cmb_nastavnik.SelectedValue  + "', '");
            naredba.Append(cmb_odeljenje.SelectedValue + "', '");
            naredba.Append(cmb_predmet.SelectedValue + "')");
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
            Load_data();
            broj_sloga = raspodela.Rows.Count - 1;
            ComboFill();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            StringBuilder naredba = new StringBuilder("UPDATE Raspodela SET ");
            naredba.Append("godina_id = '" + cmb_godina.SelectedValue + "', ");
            naredba.Append("nastavnik_id = '" + cmb_nastavnik.SelectedValue + "', ");
            naredba.Append("predmet_id = '" + cmb_predmet.SelectedValue + "', ");
            naredba.Append("odeljenje_id = '" + cmb_odeljenje.SelectedValue + "' ");
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

            Load_data();
            ComboFill();
        }
    }
}
