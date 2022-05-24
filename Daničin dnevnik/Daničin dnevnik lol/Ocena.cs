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
    public partial class Ocena : Form
    {
        DataTable dt_Ocena;
        public Ocena()
        {
            InitializeComponent();
        }

        private void Ocena_Load(object sender, EventArgs e)
        {
            cmb_GodinaPopulate();
            cmb_Predmet.Enabled = false;
            cmb_Odeljenje.Enabled = false;
            cmb_Ucenik.Enabled = false;
            cmb_Ocena.Items.Add(1);
            cmb_Ocena.Items.Add(2);
            cmb_Ocena.Items.Add(3);
            cmb_Ocena.Items.Add(4);
            cmb_Ocena.Items.Add(5);
            cmb_Ocena.Enabled = false;
            cmb_ProfesorPopulate();
        }

        private void cmb_GodinaPopulate()
        {
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Skolska_godina", veza);
            DataTable dt_godina = new DataTable();
            adapter.Fill(dt_godina);
            cmb_Godina.DataSource = dt_godina;
            cmb_Godina.ValueMember = "id";
            cmb_Godina.DisplayMember = "naziv";
            //cmb_Godina.SelectedValue = 2;
        }
        private void cmb_Godina_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmb_Godina.IsHandleCreated && cmb_Godina.Focused)
            {
                cmb_ProfesorPopulate();
                cmb_Predmet.Enabled = false;
                cmb_Predmet.SelectedIndex = -1;
                cmb_Odeljenje.Enabled = false;
                cmb_Odeljenje.SelectedIndex = -1;
                cmb_Ucenik.Enabled = false;
                cmb_Ucenik.SelectedIndex = -1;
                cmb_Ocena.Enabled = false;
                cmb_Ocena.SelectedIndex = -1;

                dt_Ocena = new DataTable();
                Grid_Ocene.DataSource = dt_Ocena;
            }
        }
        
        private void cmb_ProfesorPopulate()
        {
            SqlConnection veza = Konekcija.Connect();
            StringBuilder naredba = new StringBuilder("SELECT DISTINCT Osoba.id, ime + ' ' + prezime AS naziv FROM Osoba ");
            naredba.Append(" JOIN Raspodela ON Osoba.id = nastavnik_id ");
            naredba.Append(" WHERE godina_id = " + cmb_Godina.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_profesor = new DataTable();
            adapter.Fill(dt_profesor);
            cmb_Profesor.DataSource = dt_profesor;
            cmb_Profesor.ValueMember = "id";
            cmb_Profesor.DisplayMember = "naziv";
            cmb_Profesor.SelectedIndex = -1;
        }

        private void cmb_Profesor_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Profesor.IsHandleCreated && cmb_Profesor.Focused)
            {
                cmb_PredmetPopulate();
                cmb_Predmet.Enabled = true;
                cmb_Odeljenje.Enabled = false;
                cmb_Odeljenje.SelectedIndex = -1;
                cmb_Ucenik.Enabled = false;
                cmb_Ucenik.SelectedIndex = -1;
                cmb_Ocena.Enabled = false;
                cmb_Ocena.SelectedIndex = -1;

                dt_Ocena = new DataTable();
                Grid_Ocene.DataSource = dt_Ocena;
            }
        }

        private void cmb_PredmetPopulate()
        {
            SqlConnection veza = Konekcija.Connect();
            StringBuilder naredba = new StringBuilder("SELECT DISTINCT Predmet.id AS id, naziv FROM Predmet ");
            naredba.Append(" JOIN Raspodela ON Predmet.id = predmet_id ");
            naredba.Append(" WHERE godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" AND nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_predmet = new DataTable();
            adapter.Fill(dt_predmet);
            cmb_Predmet.DataSource = dt_predmet;
            cmb_Predmet.ValueMember = "id";
            cmb_Predmet.DisplayMember = "naziv";
            cmb_Predmet.SelectedIndex = -1;
        }

        private void cmb_Predmet_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Predmet.IsHandleCreated && cmb_Predmet.Focused)
            {
                cmb_OdeljenjePopulate();
                cmb_Odeljenje.Enabled = true;

                cmb_Ucenik.Enabled = false;
                cmb_Ucenik.SelectedIndex = -1;
                cmb_Ocena.Enabled = false;
                cmb_Ocena.SelectedIndex = -1;

                dt_Ocena = new DataTable();
                Grid_Ocene.DataSource = dt_Ocena;
            }
        }

        private void cmb_OdeljenjePopulate()
        {
            StringBuilder naredba = new StringBuilder("SELECT DISTINCT Odeljenje.id AS id, STR(razred) + '-' + indeks AS naziv FROM Odeljenje ");
            naredba.Append(" JOIN Raspodela ON Odeljenje.id = odeljenje_id ");
            naredba.Append(" WHERE Raspodela.godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" AND nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" AND predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_odeljenje = new DataTable();
            adapter.Fill(dt_odeljenje);
            cmb_Odeljenje.DataSource = dt_odeljenje;
            cmb_Odeljenje.ValueMember = "id";
            cmb_Odeljenje.DisplayMember = "naziv";
            cmb_Odeljenje.SelectedIndex = -1;
        }

        private void cmb_Odeljenje_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Odeljenje.IsHandleCreated && cmb_Odeljenje.Focused)
            {
                cmb_UcenikPopulate();
                cmb_Ucenik.Enabled = true;
                cmb_Ocena.Enabled = true;
                GridPopulate();
                try
                {
                    UcenikOcenaIdSet(0);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private void cmb_UcenikPopulate()
        {
            StringBuilder naredba = new StringBuilder("SELECT Osoba.id AS id, ime + ' ' + prezime AS naziv FROM Osoba ");
            naredba.Append(" JOIN Upisnica ON Osoba.id = osoba_id ");
            naredba.Append(" WHERE Upisnica.odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_Ucenik = new DataTable();
            adapter.Fill(dt_Ucenik);
            cmb_Ucenik.DataSource = dt_Ucenik;
            cmb_Ucenik.ValueMember = "id";
            cmb_Ucenik.DisplayMember = "naziv";
            cmb_Ucenik.SelectedIndex = -1;
        }
        private void GridPopulate()
        {
            StringBuilder naredba = new StringBuilder("SELECT Ocena.id AS id, ime + ' ' + prezime AS naziv, ocena, ucenik_id, datum FROM Osoba ");
            naredba.Append(" JOIN Ocena ON Osoba.id = ucenik_id ");
            naredba.Append(" JOIN Raspodela ON raspodela_id = Raspodela.id ");
            naredba.Append(" WHERE raspodela_id = ");
            naredba.Append(" (SELECT id FROM Raspodela ");
            naredba.Append(" WHERE godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" AND nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" AND predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            naredba.Append(" AND odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString() + ")" );
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            dt_Ocena = new DataTable();
            adapter.Fill(dt_Ocena);
            Grid_Ocene.DataSource = dt_Ocena;
            Grid_Ocene.AllowUserToAddRows = false;
            Grid_Ocene.Columns["ucenik_id"].Visible = false;
        }

        private void Grid_Ocene_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UcenikOcenaIdSet(e.RowIndex);
            }
        }
        private void UcenikOcenaIdSet(int broj_sloga)
        {
            cmb_Ucenik.SelectedValue = dt_Ocena.Rows[broj_sloga]["ucenik_id"];
            cmb_Ocena.SelectedItem = dt_Ocena.Rows[broj_sloga]["ocena"];
            txt_Id.Text = dt_Ocena.Rows[broj_sloga]["id"].ToString();
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            // INSERT 
            StringBuilder naredba = new StringBuilder("SELECT id FROM Raspodela ");
            naredba.Append(" WHERE godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" AND nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" AND predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            naredba.Append(" AND odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString());
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza);
            int id_raspodele = 0;
            try
            {
                veza.Open();
                id_raspodele = (int) komanda.ExecuteScalar();
                veza.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
            if (id_raspodele > 0)
            {
                naredba = new StringBuilder("INSERT INTO Ocena (datum, raspodela_id, ucenik_id, ocena) VALUES ('");
                DateTime datum = Datum.Value;
                naredba.Append(datum.ToString("yyyy-MM-dd") + "', '");
                naredba.Append(id_raspodele.ToString() + "', '");
                naredba.Append(cmb_Ucenik.SelectedValue.ToString() + "', '");
                naredba.Append(cmb_Ocena.SelectedItem.ToString() + "')");
                komanda = new SqlCommand(naredba.ToString(), veza);
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
            }
            GridPopulate();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            // UPDATE
            if (Convert.ToInt32(txt_Id.Text) > 0)
            {
                DateTime datum = Datum.Value;
                StringBuilder naredba = new StringBuilder("UPDATE Ocena SET ");
                naredba.Append(" ucenik_id = '" + cmb_Ucenik.SelectedValue.ToString() + "', ");
                naredba.Append(" ocena = '" + cmb_Ocena.SelectedItem.ToString() + "', ");
                naredba.Append(" datum = '" + datum.ToString("yyyy-MM-dd") + "' ");
                naredba.Append(" WHERE id = " + txt_Id.Text);
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
                GridPopulate();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            // DELETE
            if (Convert.ToInt32(txt_Id.Text) > 0)
            {
                string naredba = "DELETE FROM Ocena WHERE id = " + txt_Id.Text;
                SqlConnection veza = Konekcija.Connect();
                SqlCommand komanda = new SqlCommand(naredba, veza);
                try
                {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
                GridPopulate();
                UcenikOcenaIdSet(0);
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                }
            }
        }
    }
}
