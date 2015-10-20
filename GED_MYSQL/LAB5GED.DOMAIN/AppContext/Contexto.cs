﻿using LAB5GED.DOMAIN.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LAB5GED.DOMAIN.AppContext
{
   public class Contexto:DbContext
    {
        #region DbSets

       public DbSet<Acao> Acoes { get; set; }
       public DbSet<Caixa> Caixas { get; set; }
       public DbSet<Classe> Classes { get; set; }
       public DbSet<Corredor> Corredores { get; set; }
       public DbSet<Documento> Documentos { get; set; }
      // public DbSet<DocumentoCaixa> DocumentosCaixas { get; set; }
       public DbSet<DocumentoDigitalizacao> DocumentoDigitalizacoes { get; set; }
       public DbSet<DocumentoManipulacao> DocumentoManipulacoes { get; set; }
       public DbSet<DocumentoTipoManipulacao> DocumentoTiposManipulacao { get; set; }
       public DbSet<Estante> Estantes { get; set; }
       public DbSet<Grupo> Grupos { get; set; }
       public DbSet<Prateleira> Prateleiras { get; set; }
       public DbSet<PrazoGuarda> PrazosGuarda { get; set; }
       public DbSet<Log> Log { get; set; }
       public DbSet<Subclasse> Subclasses{ get; set; }
       public DbSet<Serie> Series { get; set; }
       public DbSet<Subserie> Subseries { get; set; }
       public DbSet<SubserieIndice> SubserieIndices { get; set; }
       public DbSet<SubserieIndiceValor> SubserieIndiceValores { get; set; }
       public DbSet<Usuario> Usuarios { get; set; }
       //public DbSet<UsuarioGrupo> UsuariosGrupos { get; set; }
 
      // public DbSet<TipoDestinacaoSubserie> TiposDestinacaoSubseries { get; set; }


 
        #endregion

        public Contexto() : base("Contexto") { }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subserie>()
                .Property(ss=>ss.Subserie_pai).IsOptional();

            modelBuilder.Entity<Documento>().Property(d => d.Caixa).IsOptional();

           
                


            #region Navigations

            //N=>N Usurio->Grupo
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Grupos)
                .WithMany()
                .Map(u=>
                    {
                        u.MapLeftKey("USUARIO");
                        u.MapRightKey("GRUPO");
                        u.ToTable("tb_usuario_grupo");
                    });

            //N=>N Acao->Grupo
            modelBuilder.Entity<Acao>()
           .HasMany(u => u.Grupos)
           .WithMany(u=>u.Acoes)
           .Map(u =>
           {
               u.MapLeftKey("ACAO");
               u.MapRightKey("GRUPO");
               u.ToTable("tb_acao_grupo");
           });

            //N=>N Grupo->Acao
           modelBuilder.Entity<Grupo>()
          .HasMany(u => u.Acoes)
          .WithMany(u=>u.Grupos)
          .Map(u =>
          {
          u.MapLeftKey("GRUPO");
          u.MapRightKey("ACAO");
          u.ToTable("tb_acao_grupo");
         });

           modelBuilder.Entity<Usuario>()
               .HasMany(u => u.Subseries)
               .WithMany(u => u.Usuarios)
               .Map(u =>
               {
                   u.MapLeftKey("USUARIO");
                   u.MapRightKey("SUBSERIE");
                   u.ToTable("tb_subserie_usuario");
               });

           //N=>1 Documento->Caixa
            modelBuilder.Entity<Documento>()
           .HasOptional(c => c.FK_Caixa)
           .WithMany(d=>d.Documentos)
           .HasForeignKey(d=>d.Caixa);


            #endregion

           base.OnModelCreating(modelBuilder);
            base.Configuration.LazyLoadingEnabled = true;
           
        }
    }
}
