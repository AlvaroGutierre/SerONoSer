using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using Entidades;
using CapaNegocio;

namespace Tablero
{
    public partial class FrmJuego : Form
    {
        GestionDS gestionDs;
        GestionNg gestionNg;
        int sec = 0;
        int nivel;
        int acertadas = 0;
        int fallidas = 0;
        

        public FrmJuego()
        {
            InitializeComponent();
        }

        private void btnComenzar_Click(object sender, EventArgs e)
        {
            gestionNg = new GestionNg();
            if (int.TryParse(txtNivel.Text,out nivel))
            {
                btn1.Enabled = true;
                btn2.Enabled = true;
                btn3.Enabled = true;
                btn4.Enabled = true;
                btn5.Enabled = true;
                btn6.Enabled = true;
                btn7.Enabled = true;
                btn8.Enabled = true;
                btn9.Enabled = true;
                btn10.Enabled = true;
                btn11.Enabled = true;
                btn12.Enabled = true;
                gestionNg.ObtenerPreguntas(nivel, out string error);
                lblNivel.Text = nivel.ToString();
                if (!error.Equals(""))
                {
                    MessageBox.Show(error);
                    return;
                }
                NuevaPregunta();
            }
            else
            {
                MessageBox.Show("Has de introducir un número en el nivel");
            }
            
        }
        private void NuevaPregunta()
        {
            lblRespuestaValida.Text = null;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                string tipo = this.Controls[i].GetType().ToString();
                if (tipo.Equals("System.Windows.Forms.Button"))
                {
                    Button boton = (Button)this.Controls[i];
                    if (boton.Tag != null && !boton.Text.Equals("&Finalizar") && !boton.Text.Equals("Re/&Comenzar"))
                    {
                        boton.Tag = "";
                        boton.BackColor = default;
                        boton.Enabled = true;
                    }
                }
            }
            acertadas = 0;
            fallidas = 0;
            PreguntaDTO pregunta = gestionNg.PreguntaNivel();
            if (pregunta == null)
            {
                nivel++;
                gestionNg.ObtenerPreguntas(nivel, out string error);
                if (!error.Equals(""))
                {
                    MessageBox.Show("Se han acabado los niveles");
                    this.Close();
                    return;
                }

                NuevaPregunta();

                lblNivel.Text = nivel.ToString();
                return;
            }
            lblEnunciado.Text = pregunta.Enunciado;
            int x = 0;

            for (int i = 0; i < this.Controls.Count; i++)
            {
                string tipo = this.Controls[i].GetType().ToString();
                if (tipo.Equals("System.Windows.Forms.Button"))
                {
                    Button boton = (Button)this.Controls[i];
                    if (!boton.Text.Equals("&Finalizar") && !boton.Text.Equals("Re/&Comenzar"))
                    {
                        boton.Text = pregunta.Respuestas[x].PosibleRespuesta;
                        boton.Tag= pregunta.Respuestas[x++].Explicacion;
                    }
                }
            }

            sec = 15;
            lblTiempo.Text = sec.ToString();
            tmrTiempoTotal.Interval = 1000;
            tmrTiempoTotal.Start();
        }

        private void FrmJuego_Load(object sender, EventArgs e)
        {
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn10.Enabled = false;
            btn11.Enabled = false;
            btn12.Enabled = false;

            gestionDs = new GestionDS(out string mensaje);
            if (!mensaje.Equals(""))
            {
                MessageBox.Show(mensaje);
                this.Close();
            }
            
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button boton = sender as Button;
            boton.Enabled = false;
            if (boton.Tag != null && String.IsNullOrEmpty(boton.Tag.ToString()))
            {
                boton.BackColor = Color.Green;
                lblRespuestaValida.Text = null;
                acertadas+=1;
                if (acertadas == 8)
                {
                    tmrTiempoTotal.Stop();
                    if (MessageBox.Show("Has acertado las 8 respuestas válidas, ¿deseas continuar con otra pregunta?", "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        NuevaPregunta();
                    }
                }
            }
            else
            {
                boton.BackColor = Color.Red;
                lblRespuestaValida.Text = boton.Tag.ToString();
                fallidas++;
                if (fallidas == 4)
                {
                    tmrTiempoTotal.Stop();
                    if (MessageBox.Show("Has fallado las 4 respuestas erróneas, ¿deseas continuar con otra pregunta?", "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        NuevaPregunta();
                    }
                }
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tmrTiempoTotal_Tick(object sender, EventArgs e)
        {
            sec--;
            lblTiempo.Text = sec.ToString();
            if (sec == 0)
            {
                tmrTiempoTotal.Stop();

                if (MessageBox.Show("Te has quedado sin tiempo para esta pregunta, ¿deseas continuar con otra pregunta? No finalizará el programa", "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NuevaPregunta();
                }
            }

        }
    }
}