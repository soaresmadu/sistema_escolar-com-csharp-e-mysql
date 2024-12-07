using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlConnector;

namespace SistemaEscolar
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            txtApresentacao.Text = "Olá semideus! \n Faça o login.";
            lblErro.Text = "";
        }

        private void verificarUsuario(string user, string pass)
        {
            if (user == "" || user == null || pass == "" || pass == null)
            {
                lblErro.Text = "Por favor, preencha todos os campos!";
                return;
            }

            string connectionString = "Server=localhost;Database=escola_sistema;Uid=root;Pwd=Qcf@1316;";

            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM usuario WHERE nome_usuario = @usuario AND senha = @senha;";

                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection)) 
                    {
                        cmd.Parameters.AddWithValue("@usuario", user);
                        cmd.Parameters.AddWithValue("@senha", pass);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            this.Hide();
                            Form2 mainForm = new Form2();
                            mainForm.Show();
                        }
                        else
                        {
                            lblErro.Text =  "Usuário ou senha inválidos! \n Tente novamente.";
                        }
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                lblErro.Text = "Erro ao se conectar com o banco de dados. \n Feche o programa e tente novamente mais tarde. \n" + ex;
            }
            catch (Exception)
            {
                lblErro.Text = "Erro ao se conectar com o banco de dados. \n Feche o programa e tente novamente mais tarde. \n";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;
            verificarUsuario(usuario, senha);
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                string usuario = txtUsuario.Text;
                string senha = txtSenha.Text;
                verificarUsuario(usuario, senha);
            }
        }
    }
}
