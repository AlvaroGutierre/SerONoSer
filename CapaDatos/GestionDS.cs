using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.DsQuizTableAdapters;
using Entidades;

namespace CapaDatos
{
    public class GestionDS
    {
        DsQuiz dsQuiz = new DsQuiz();
        int maxNivel = 0;
        public GestionDS(out string mensaje)
        {
            mensaje = LlenarTablas();
        }

        private string LlenarTablas()
        {
            string nivel = "";
            DsQuizTableAdapters.PreguntasTableAdapter daPregunta = new DsQuizTableAdapters.PreguntasTableAdapter();
            RespuestasTableAdapter daRespuesta = new RespuestasTableAdapter();
            RespNoValidasTableAdapter daRespuestaNoValida = new RespNoValidasTableAdapter();
            try
            {
                daPregunta.Fill(dsQuiz.Preguntas);
                daRespuesta.Fill(dsQuiz.Respuestas);
                daRespuestaNoValida.Fill(dsQuiz.RespNoValidas);

                if (daPregunta.GetData().Count==0)
                {
                    return "No hay pregutnas";
                }
                else
                {
                    foreach (var pregunta in dsQuiz.Preguntas)
                    {
                        if (pregunta.GetRespuestasRows().Length!=12)
                        {
                            return "Tiene menos de 12 respuestas, tiene "+ pregunta.GetRespuestasRows().Length;
                        }
                        else
                        {
                            int cnt = 0;
                            foreach (var respuestas in pregunta.GetRespuestasRows())
                            {
                                if (respuestas.Valida)
                                {
                                    cnt = cnt + 1;
                                }
                            }
                            if (cnt<8)
                            {
                                return "No tiene 8 validas y 4 incorrectas";
                            }
                            nivel = nivel + pregunta.Nivel;
                            if (pregunta.Nivel>maxNivel)
                            {
                                maxNivel = pregunta.Nivel;
                            }
                        }
                    }
                    for (int i = 1; i < maxNivel; i++)
                    {
                        if (!nivel.Contains(Char.Parse(i.ToString())))
                        {
                            return "No hay preguntas de nivel " + i;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return "";
        }
        public List<PreguntaDTO> PreguntasNivel(int nivel)
        {
            RespNoValidasTableAdapter daRespuestaNoValida = new RespNoValidasTableAdapter();
            if (nivel>maxNivel)
            {
                return null;
            }
            else
            {

                return dsQuiz.Preguntas.Where(drPregunta => drPregunta.Nivel == nivel).Select(drPregunta => new PreguntaDTO(drPregunta.NumPregunta, drPregunta.Enunciado, nivel, drPregunta.GetRespuestasRows().Select(drRespuesta => new RespuestaDTO(drRespuesta.NumPregunta, drRespuesta.NumRespuesta, drRespuesta.PosibleRespuesta, drRespuesta.Valida, drRespuesta.GetRespNoValidasRows().Count() > 0 ? drRespuesta.GetRespNoValidasRows().Select(drRespNoValida => drRespNoValida.Explicacion).SingleOrDefault() : "")).ToArray())).ToList();
            
        }

    }
}
