INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO, ANULADO)

-- Fila para el DEBITO
SELECT 
    'CC' AS TIPO, -- Valor fijo
    ROW_NUMBER() OVER (ORDER BY fecha, nit_propietario_cuenta) AS NUMERO, -- Generamos un número incremental basado en fecha y tercero
    '110505002' AS CUENTA, -- Cuenta para el DEBITO
    nit_propietario_cuenta AS TERCERO, -- TERCERO desde FactOpcaja
    'CONSIGNACION CAJA CONSTRIBUCIONES' AS DETALLE, -- Detalle fijo
    30000 AS DEBITO, -- Valor de DEBITO
    0 AS CREDITO, -- Sin valor en CREDITO
    0 AS BASE, -- BASE se establece en 0
    '001' AS CCOSTO, -- Centro de costos
    fecha AS FECHAMOVIMIENTO, -- FECHAMOVIMIENTO desde FactOpcaja
    NULL AS DOCUMENTO, -- DOCUMENTO como NULL
    NULL AS ANULADO -- ANULADO como NULL
FROM FactOpcaja

UNION ALL

-- Fila para el primer CREDITO (20,000)
SELECT 
    'CC' AS TIPO, -- Valor fijo
    ROW_NUMBER() OVER (ORDER BY fecha, nit_propietario_cuenta) AS NUMERO, -- Generamos un número incremental basado en fecha y tercero
    '310505001' AS CUENTA, -- Cuenta para el primer CREDITO
    nit_propietario_cuenta AS TERCERO, -- TERCERO desde FactOpcaja
    'CONSIGNACION CAJA CONSTRIBUCIONES' AS DETALLE, -- Detalle fijo
    0 AS DEBITO, -- Sin valor en DEBITO
    20000 AS CREDITO, -- Valor del primer CREDITO
    0 AS BASE, -- BASE se establece en 0
    '001' AS CCOSTO, -- Centro de costos
    fecha AS FECHAMOVIMIENTO, -- FECHAMOVIMIENTO desde FactOpcaja
    NULL AS DOCUMENTO, -- DOCUMENTO como NULL
    NULL AS ANULADO -- ANULADO como NULL
FROM FactOpcaja

UNION ALL

-- Fila para el segundo CREDITO (10,000)
SELECT 
    'CC' AS TIPO, -- Valor fijo
    ROW_NUMBER() OVER (ORDER BY fecha, nit_propietario_cuenta) AS NUMERO, -- Generamos un número incremental basado en fecha y tercero
    '213005001' AS CUENTA, -- Cuenta para el segundo CREDITO
    nit_propietario_cuenta AS TERCERO, -- TERCERO desde FactOpcaja
    'CONSIGNACION CAJA CONSTRIBUCIONES' AS DETALLE, -- Detalle fijo
    0 AS DEBITO, -- Sin valor en DEBITO
    10000 AS CREDITO, -- Valor del segundo CREDITO
    0 AS BASE, -- BASE se establece en 0
    '001' AS CCOSTO, -- Centro de costos
    fecha AS FECHAMOVIMIENTO, -- FECHAMOVIMIENTO desde FactOpcaja
    NULL AS DOCUMENTO, -- DOCUMENTO como NULL
    NULL AS ANULADO -- ANULADO como NULL
FROM FactOpcaja;
