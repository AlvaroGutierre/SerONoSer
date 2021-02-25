using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class GestionNg
    {
        List<PreguntaDTO> preguntas = new List<PreguntaDTO>();
        Random rnd;

        public GestionNg()
        {
            rnd = new Random();
        }

        public void ObtenerPreguntas(int nivel, out string error)
        {
            error = "";
            GestionDS gestion = new GestionDS(out string msg);
            preguntas = gestion.PreguntasNivel(nivel);
            if (preguntas==null)
            {
                error = "No hay preguntas de el nivel "+ nivel;
            }
            
        }
        public PreguntaDTO PreguntaNivel()
        {
            PreguntaDTO pregunta = new PreguntaDTO();
            int num = preguntas.Count;
            if (num == 0)
            {
                return null;
            }
            int numero=rnd.Next(0, num);
            pregunta = preguntas[numero];
            preguntas.RemoveAt(numero);
            return pregunta;
            

        }
    }
}
