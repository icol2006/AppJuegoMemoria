using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppJuegoAdivina
{
    public partial class Form1 : Form
    {
        List<Int32> listadoOrden = new List<Int32>();
        List<Int32> listadoOrdenRespondido = new List<Int32>();
        Boolean revisionResultado = false;
        Task cambiarColorLabelSeleccionado;

        int cantidadNumeros = 3;
        int cantidadOpcionesEscogidas = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            lblSeleccionar.Visible = false;
            txtResultado.Text = "";
            generarNumeros();
            btnJugar.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < listadoOrden.Count; i++)
            {
                backgroundWorker1.ReportProgress(listadoOrden[i]);
                Thread.Sleep(700);
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(700);
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           switch  (e.ProgressPercentage){
               case 1:
                   lblOpcion1.BackColor = Color.Yellow;
               break;

               case 2:
               lblOpcion2.BackColor = Color.Yellow;
               break;

               case 3:
               lblOpcion3.BackColor = Color.Yellow;
               break;

               default:
               lblOpcion1.BackColor = Color.Black;
               lblOpcion2.BackColor = Color.Black;
               lblOpcion3.BackColor = Color.Black;
               break;
          
           }


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnJugar.Enabled = true;
            lblSeleccionar.Visible = true;
        }

        public void generarNumeros()
        {
            Random random = new Random();
            listadoOrden.Clear();


            if (revisionResultado == true)
            {
                cantidadNumeros += 1;
                revisionResultado = false;
            }
            
            for (int i = 0; i < cantidadNumeros; i++)
            {
                int randomNumber = random.Next(1, 4);
                listadoOrden.Add(randomNumber);
            }
        }

        private void lblOpcion1_Click(object sender, EventArgs e)
        {
            agregarRespuestaRespondida(1);
             cambiarColorLabelSeleccionado = new Task(() => this.actualizarLabel(1));
             cambiarColorLabelSeleccionado.Start();
        }

        private void lblOpcion2_Click(object sender, EventArgs e)
        {
            agregarRespuestaRespondida(2);
             cambiarColorLabelSeleccionado = new Task(() => this.actualizarLabel(2));
             cambiarColorLabelSeleccionado.Start();
        }

        private void lblOpcion3_Click(object sender, EventArgs e)
        {
            agregarRespuestaRespondida(3);
             cambiarColorLabelSeleccionado = new Task(() => this.actualizarLabel(3));
             cambiarColorLabelSeleccionado.Start();
        }

        public void agregarRespuestaRespondida(int respuesta)
        {
            if (cantidadOpcionesEscogidas < cantidadNumeros)
            {
                listadoOrdenRespondido.Add(respuesta);
                cantidadOpcionesEscogidas++;
            }

            if(cantidadOpcionesEscogidas==cantidadNumeros)
            {
                verificarResultado();
            }

        }

        public void verificarResultado()
        {
            //Comparar listadoOrden y listadoOrdenRespondido
              if (listadoOrden.SequenceEqual(listadoOrdenRespondido))
            {
                revisionResultado = true;
                txtResultado.Text = "Correcto";
                lblNivel.Text = (cantidadNumeros - 1)+"";
            }
            else
            {
                int valoresIncorrectos=0;
               IEnumerable<Int32> differenceQuery = listadoOrden.Except(listadoOrdenRespondido);
                  
             /*
            foreach(int s in differenceQuery)
            valoresIncorrectos++;*/

           // txtResultado.Text = "Incorrecto, cantidad de valores incorrectos=" + valoresIncorrectos;
            txtResultado.Text = "Incorrecto";
            }

            listadoOrdenRespondido.Clear();
            cantidadOpcionesEscogidas = 0;
        }

        private void empezarJuegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cantidadNumeros = 3;
            lblNivel.Text = "1";
        }

        public void actualizarLabel(int respuesta)
        {

            switch (respuesta)
            {
                case 1:
            Invoke(new Action(()=>lblOpcion1.BackColor=Color.Blue)); 
            Thread.Sleep(300);
                        Invoke(new Action(()=>lblOpcion1.BackColor=Color.Black)); 
                    break;
                case 2:
            Invoke(new Action(()=>lblOpcion2.BackColor=Color.Blue)); 
            Thread.Sleep(300);
                        Invoke(new Action(()=>lblOpcion2.BackColor=Color.Black)); 
                    break;

                case 3:
           Invoke(new Action(()=>lblOpcion3.BackColor=Color.Blue)); 
            Thread.Sleep(300);
                        Invoke(new Action(()=>lblOpcion3.BackColor=Color.Black)); 
                    break;
            }

        
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}
