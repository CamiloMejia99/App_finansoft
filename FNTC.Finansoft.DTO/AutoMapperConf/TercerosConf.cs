namespace FNTC.Finansoft.Accounting.DTO.AutoMapperConf
{
    public static class TercerosConf
    {
        /*
        public static  bool IsConfigured { get; set; }

        public static void Configure()
        {
            IsConfigured = true;

            Mapper.Initialize
                (cfg =>
                {
                    #region Tercero
                    cfg.CreateMap<Tercero2DTO, Tercero>()
                        //.ForMember(
                        //src => src.NombreComercial
                        //, opt => opt.MapFrom(src => src.Nombre))
                        //.ForMember(
                        //src => src.NOMBRE
                        //, opt => opt.MapFrom(src => src.Nombre))
                              ;

                    cfg.CreateMap<Tercero, Tercero2DTO>()
                        //.ForMember(
                        //src => src.NombreComercial
                        //, opt => opt.MapFrom(src => src.Nombre))
                        //.ForMember(
                        //src => src.NOMBRE
                        //, opt => opt.MapFrom(src => src.Nombre));
                        ;
                    #endregion

                    #region Tercero.AdministradoraDTO
                    cfg.CreateMap<Administradora, AdministradoraDTO>()
                                  .ForMember(
                                  src => src.Nombre
                                  , opt => opt.MapFrom(src => src.NOMBRE));

                    cfg.CreateMap<AdministradoraDTO, Administradora>()
                        .ForMember(
                        src => src.NombreComercial
                        , opt => opt.MapFrom(src => src.Nombre))
                        
                        .ForMember(
                        src => src.NOMBRE
                        , opt => opt.MapFrom(src => src.Nombre)); 
                    #endregion
                }
                );
            //test mapper
            //var terceros = new AsociadosContext().Terceros.ToList();
            //var tercerosDTO = new List<TerceroDTO>();

            //var dto = Mapper.Map<TerceroDTO>(terceros.First());


            //foreach (var item in terceros)
            //{
            //    tercerosDTO.Add( Mapper.Map<TerceroDTO>(item));
            //}

            //Console.ReadKey();


            //var administradra = new AsociadosContext().Administradoras.First();

            //AdministradoraDTO administradoraDto =
            //    Mapper.Map<AdministradoraDTO>(administradra);

            //var revere = Mapper.Map<Administradora>(administradoraDto);
        }

        //old
        public static void ConfigureInstance()
        {
            var configToDTO =
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Administradora, AdministradoraDTO>()
                        .ForMember(
                        src => src.Nombre
                        , opt => opt.MapFrom(src => src.NOMBRE));


                    cfg.CreateMap<AdministradoraDTO, Administradora>()
                        .ForMember(
                        src => src.NombreComercial
                        , opt => opt.MapFrom(src => src.Nombre))
                        .ForMember(
                        src => src.NOMBRE
                        , opt => opt.MapFrom(src => src.Nombre))
                        ;
                    //.
                }
                    );





            var mapper = configToDTO.CreateMapper();

            //var administradra = new AsociadosContext().Administradoras.First();

            //AdministradoraDTO administradoraDto =
            //    mapper.Map<AdministradoraDTO>(administradra);

            //var revere = mapper.Map<Administradora>(administradoraDto);
        }
        */
    }
}
