﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pfeAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbQuantusEntities : DbContext
    {
        public dbQuantusEntities()
            : base("name=dbQuantusEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<pfeEMPRESA> pfeEMPRESA { get; set; }
        public virtual DbSet<pfeFACTURAS> pfeFACTURAS { get; set; }
        public virtual DbSet<pfeFACTURASDET> pfeFACTURASDET { get; set; }
        public virtual DbSet<pfeFILIALES> pfeFILIALES { get; set; }
        public virtual DbSet<pfeORDENCOMPRA> pfeORDENCOMPRA { get; set; }
        public virtual DbSet<pfePARAMETROS> pfePARAMETROS { get; set; }
        public virtual DbSet<pfePROVEEDORES> pfePROVEEDORES { get; set; }
        public virtual DbSet<pfeUSUARIOS> pfeUSUARIOS { get; set; }
    }
}