//using FNTC.Finansoft.Accounting.DTO;
//using FNTC.Finansoft.Accounting.DTO.Nomina;
//using System;

//namespace FNTC.Finansoft.Accounting.DAL.Model.Nomina
//{
//    using System.ComponentModel.DataAnnotations.Schema;
//    [Table("nom.EnvioPlanos")]

//    public class EnvioPlanoDAL
//    {
//        public bool CreateEnvi(EnvioPlano nuevoEnvio)
//        {
//            //el nombre debe existir
//            if (String.IsNullOrEmpty(nuevoEnvio.EMPRESA))
//            {
//                throw new ArgumentNullException("EMPRESA");
//            }
//            if (String.IsNullOrEmpty(nuevoEnvio.PERDEDUCC))
//            {
//                throw new ArgumentNullException("PERDEDUCC");
//            }

//            if (String.IsNullOrEmpty(nuevoEnvio.ORDEND))
//            {
//                throw new ArgumentNullException("ORDEND");
//            }
//            if (String.IsNullOrEmpty(nuevoEnvio.PLANO))
//            {
//                throw new ArgumentNullException("PLANO");
//            }
//            if (String.IsNullOrEmpty(nuevoEnvio.RUTA))
//            {
//                throw new ArgumentNullException("RUTA");
//            }
//            if (String.IsNullOrEmpty(nuevoEnvio.CONINC))
//            {
//                throw new ArgumentNullException("CONINC");
//            }


//            using (var ctx = new AccountingContext())
//            {
//                ctx.EnvioPlano.Add(nuevoEnvio);
//                try
//                {
//                    return ctx.SaveChanges() > 0 ? true : false;
//                }
//                catch (Exception e)
//                {
//                    throw;
//                }
//            }
//        }

//        public bool UpdateDisc(EnvioPlano updateobjeto)
//        {
//            //el nombre debe existir
//            if (String.IsNullOrEmpty(updateobjeto.EMPRESA))
//            {
//                throw new ArgumentNullException("EMPRESA");
//            }
//            if (String.IsNullOrEmpty(updateobjeto.PERDEDUCC))
//            {
//                throw new ArgumentNullException("PERDEDUCC");
//            }

//            if (String.IsNullOrEmpty(updateobjeto.ORDEND))
//            {
//                throw new ArgumentNullException("ORDEND");
//            }
//            if (String.IsNullOrEmpty(updateobjeto.PLANO))
//            {
//                throw new ArgumentNullException("PLANO");
//            }
//            if (String.IsNullOrEmpty(updateobjeto.RUTA))
//            {
//                throw new ArgumentNullException("RUTA");
//            }
//            if (String.IsNullOrEmpty(updateobjeto.CONINC))
//            {
//                throw new ArgumentNullException("CONINC");
//            }

//            using (var ctx = new AccountingContext())
//            {
//                ctx.Entry(updateobjeto).State = System.Data.Entity.EntityState.Modified;

//                try
//                {
//                    return ctx.SaveChanges() > 0 ? true : false;
//                }
//                catch (Exception e)
//                {

//                    throw;
//                }
//            }


//        }

//        public bool DeleteDisc(EnvioPlano EnvioDelete)
//        {
//            using (var ctx = new AccountingContext())
//            {
//                try
//                {
//                    //si existe
//                    if (ctx.EnvioPlano.Find(EnvioDelete.ID) != null)
//                    {
//                        ctx.EnvioPlano.Remove(EnvioDelete);
//                    }
//                    return ctx.SaveChanges() > 0 ? true : false;
//                }
//                catch (Exception e)
//                {
//                    throw;
//                }
//            }

//        }

//    }
//}
