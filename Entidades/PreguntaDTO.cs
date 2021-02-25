using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PreguntaDTO : IEquatable<PreguntaDTO>
    {
        public PreguntaDTO()
        {
        }

        public PreguntaDTO(int numPregunta, string enunciado, int nivel, RespuestaDTO[] respuestas)
        {
            NumPregunta = numPregunta;
            Enunciado = enunciado;
            Nivel = nivel;
            Respuestas = respuestas;
        }

        public int NumPregunta { get; set; }
        public string Enunciado { get; set; }
        public int Nivel { get; set; }
        public RespuestaDTO[] Respuestas { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PreguntaDTO);
        }

        public bool Equals(PreguntaDTO other)
        {
            return other != null &&
                   NumPregunta == other.NumPregunta;
        }
    }
}
