
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Helper
{
    public static class SubirArchivos 
    {
        // Configuracion
        private static int mTamañoFoto = 150000;//150K
        private static string mMimeFoto = "image/jpeg";
        private static string[] mMimeDoc = new string [] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
        private static int mTamañoDoc = 6000000;//6M
        private static string mRutaBase = "~/Uploads/";
        private static string mRutaPerfil = "/FotoPerfil/";
        private static string mRutaAdjuntos = "/Adjuntos/";
        private static string mRutaGuardar;
        private static string[] mResultado;
        private static HttpPostedFileBase mArchivo;

        /// <summary>
        /// Validar archivos y subirlos al servidor
        /// </summary>
        /// <param name="Usuario_id">Id del usuario</param>
        /// <param name="Foto"> Arichivo HttpPostedFileBase recogido del POST</param>
        /// <returns> devuelve una String Array de 2 valores, en [0] cualquier tipo de error, si no, devuelve null..
        /// Si [0] es null....... [1] devuelve el nombre del fichero
        /// </returns>
        public static string[] SubirFotoPerfil(int Usuario_id, HttpPostedFileBase Foto)
        {
            mResultado = new string[2];
            mArchivo = Foto;
            mRutaGuardar = "";
            try
            {
                mResultado[0] = ValidarArchivo(1);
                if (mResultado[0] == null)
                {
                   
                    string destino = HttpContext.Current.Server.MapPath(mRutaBase + Usuario_id);
                    if (!Directory.Exists(destino))
                    {
                        Directory.CreateDirectory(destino);
                        Directory.CreateDirectory(destino + mRutaPerfil);
                    }
                    if (!Directory.Exists(destino + mRutaPerfil))
                    {
                        Directory.CreateDirectory(destino + mRutaPerfil);
                    }
                    mRutaGuardar = mRutaBase + Usuario_id + mRutaPerfil;
                    mResultado[0] = SubirAServer(Usuario_id);
                }
               
            }
            catch (Exception ex)
            {
                mResultado[0] = ex.Message;


                return mResultado;
            }
            return mResultado;
        }
        /// <summary>
        /// Validar documentos y subirlos al servidor
        /// </summary>
        /// <param name="Usuario_id">Id del usuario</param>
        /// <param name="Foto"> Arichivo HttpPostedFileBase recogido del POST</param>
        /// <returns> devuelve una String Array de 2 valores, en [0] cualquier tipo de error, si no, devuelve null..
        /// Si [0] es null....... [1] devuelve el nombre del fichero
        public static string[] SubirAdjuntos(int Usuario_id, HttpPostedFileBase Doc)
        {
            mResultado = new string[2];
            mArchivo = Doc;
            mRutaGuardar = "";
            try
            {
                mResultado[0] = ValidarArchivo(2);
                if (mResultado[0] == null)
                {

                    string destino = HttpContext.Current.Server.MapPath(mRutaBase + Usuario_id);
                    if (!Directory.Exists(destino))
                    {
                        Directory.CreateDirectory(destino);
                        Directory.CreateDirectory(destino + mRutaAdjuntos);
                    }
                    if (!Directory.Exists(destino + mRutaAdjuntos))
                    {
                        Directory.CreateDirectory(destino + mRutaAdjuntos);
                    }
                    mRutaGuardar = mRutaBase + Usuario_id + mRutaAdjuntos;
                    mResultado[0] = SubirAServer(Usuario_id);
                }

            }
            catch (Exception ex)
            {
                mResultado[0] = ex.Message;


                return mResultado;
            }
            return mResultado;
        }
        private static string SubirAServer(int id)
        {
            string result = null;
            try
            {
                // renombro archivo
                string archivo = id + DateTime.Now.ToString("ddmmss")
                  + Path.GetFileName(mArchivo.FileName);
                //subo
                mArchivo.SaveAs(HttpContext.Current.Server.MapPath(mRutaGuardar + archivo));
                mResultado[1] = archivo;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
            return result;
          
           
        }
        private static string ValidarArchivo(int tipo)
        {
            string result = null;
            if (tipo == 1)//fotos
            {
                if (mMimeFoto != mArchivo.ContentType)
                    result = "Solo se admiten images en formato JPEG";
                else if (mArchivo.ContentLength> mTamañoFoto)
                    result = "Imagen muy grande. Maximo 150K";
            } else if (tipo == 2)//Documentos
            {
              //  string extension = Path.GetExtension(mArchivo.ContentType);
                int existe = Array.IndexOf(mMimeDoc, mArchivo.ContentType);
                if (existe < 0)
                    result = "Solo adminte Documentos pdf y docx";
                else if (mArchivo.ContentLength > mTamañoDoc)
                    result = "Documento muy grande. Maximo 6M";
            }
            return result;
        }
        public static void BorrarFotoAntigua(int Usuario_id, string foto)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(mRutaBase + Usuario_id + mRutaPerfil);
                string fullPath = Path.Combine(path, foto);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static void BorrarAdjunto(int Usuario_id, string fichero)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(mRutaBase + Usuario_id + mRutaAdjuntos);
            string fullPath = Path.Combine(path, fichero);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
        public static string GetRutaFotoPerfil()
        {
            return mRutaBase + SesionHelper.GetUser() + mRutaPerfil;
        }
        public static string GetRutaAdjuntos()
        {
            return mRutaBase + SesionHelper.GetUser() + mRutaAdjuntos;
        }
    } 
}
