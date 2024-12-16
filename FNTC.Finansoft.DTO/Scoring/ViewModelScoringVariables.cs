using System.Collections.Generic;


namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    public class ViewModelScoringVariables

    {

        public IEnumerable<ScoringVariableAgencia> ScoringVariableAgencia { get; set; }
        public IList<ScoringVariableCategoria> ScoringVariableCategoria { get; set; }
        public IList<ScoringVariableAntiguedadCooperativa> ScoringVariableAntiguedadCooperativa { get; set; }
        public IList<ScoringVariableCapacidadPago> ScoringVariableCapacidadPago { get; set; }
        public IList<ScoringVariableEdad> ScoringVariableEdad { get; set; }
        public IList<ScoringVariableEstadosCivil> ScoringVariableEstadosCivil { get; set; }
        public IList<ScoringVariableEstrato> ScoringVariableEstrato { get; set; }
        public IList<ScoringVariableFormaPago> ScoringVariableFormaPago { get; set; }
        public IList<ScoringVariableGarantia> ScoringVariableGarantia { get; set; }
        public IList<ScoringVariableAntiguedadLaboral> ScoringVariableAntiguedadLaboral { get; set; }
        public IList<ScoringVariableIngresosTotal> ScoringVariableIngresosTotal { get; set; }
        public IList<ScoringVariableMesesPlazo> ScoringVariableMesesPlazo { get; set; }
        public IList<ScoringVariableMonto> ScoringVariableMonto { get; set; }
        public IList<ScoringVariableNivelesEducativo> ScoringVariableNivelesEducativo { get; set; }
        public IList<ScoringVariableOcupacion> ScoringVariableOcupacion { get; set; }
        public IList<ScoringVariablePersonasACargo> ScoringVariablePersonasACargo { get; set; }
        public IList<ScoringVariableReestructurado> ScoringVariableReestructurado { get; set; }
        public IList<ScoringVariableSexo> ScoringVariableSexo { get; set; }
        public IList<ScoringVariableTipoContrato> ScoringVariableTipoContrato { get; set; }
        public IList<ScoringVariableTipoVivienda> ScoringVariableTipoVivienda { get; set; }



    }
}
