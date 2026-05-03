
Public Class negBuscarDatosPedidos

    Inherits NegClaseBase.NegClaseBase


    Private _pk As String
    Private _codigo As String
    Private _cliente As String
    Private _sucursal As String
    Private _fecha As String
    Private _fechaentrega As String
    Private _referencia As String
    Private _vendedor As String
    Private _contribuyente As String
    Private _tipolista As String



    Dim _DtsDatos As New DataTable




    Public Property DtsDatosDetalle() As DataTable
        Get
            Return _DtsDatos
        End Get
        Set(ByVal value As DataTable)
            _DtsDatos = value
        End Set
    End Property


    Public Property fechaentrega() As String
        Get
            Return _fechaentrega
        End Get
        Set(ByVal value As String)
            _fechaentrega = value
        End Set
    End Property


    Public Property fecha() As String
        Get
            Return _fecha
        End Get
        Set(ByVal value As String)
            _fecha = value
        End Set
    End Property

  

    Public Property pk() As String
        Get
            Return _pk
        End Get
        Set(ByVal value As String)
            _pk = value
        End Set
    End Property

    Public Property codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property cliente() As String
        Get
            Return _cliente
        End Get
        Set(ByVal value As String)
            _cliente = value
        End Set
    End Property

    Public Property sucursal() As String
        Get
            Return _sucursal
        End Get
        Set(ByVal value As String)
            _sucursal = value
        End Set
    End Property


    Public Property referencia() As String
        Get
            Return _referencia
        End Get
        Set(ByVal value As String)
            _referencia = value
        End Set
    End Property



    Public Property vendedor() As String
        Get
            Return _vendedor
        End Get
        Set(ByVal value As String)
            _vendedor = value
        End Set
    End Property


    Public Property contribuyente() As String
        Get
            Return _contribuyente
        End Get
        Set(ByVal value As String)
            _contribuyente = value
        End Set
    End Property


    Public Property tipolista() As String
        Get
            Return _tipolista
        End Get
        Set(ByVal value As String)
            _tipolista = value
        End Set
    End Property





    Public Function Buscar(ByRef con As SqlClient.SqlConnection) As DataTable

        Dim dtsDatos As New DataTable

        MyBase.LimpiarParametrosStore()
        MyBase.AgregarParametroStore("@pk", pk, DbType.String)
        MyBase.AgregarParametroStore("@codigo", codigo, DbType.String)
        MyBase.AgregarParametroStore("@cliente", cliente, DbType.String)
        MyBase.AgregarParametroStore("@sucursal", sucursal, DbType.String)
        MyBase.AgregarParametroStore("@referencia", referencia, DbType.String)
        MyBase.AgregarParametroStore("@vendedor", vendedor, DbType.String)
        MyBase.AgregarParametroStore("@contribuyente", contribuyente, DbType.String)
        MyBase.AgregarParametroStore("@tipolista", tipolista, DbType.String)
        MyBase.AgregarParametroStore("@FechaOrden", fecha, DbType.Date)
        MyBase.AgregarParametroStore("@FechaLlegada", fechaentrega, DbType.Date)

        MyBase.AgregarParametroStore("@Detalle", FLDatosDetalle(DtsDatosDetalle), DbType.String)

        'MsgBox(FLDatosDetalle(DtsDatosDetalle))
        dtsDatos = MyBase.EjecutarBusqueda("spArmaDatosparaXMLOrdenVenta", con, True)


        Return dtsDatos

    End Function

    Private Function FLDatosDetalle(ByVal datos As DataTable)

        Dim strReturn As String
        strReturn = "x"
        Dim rs As DataRow

        Dim primero As Boolean

        '0 secuencia
        '1 item
        '2 um
        '3 clase
        '4 bodega
        '5 cantidad

        primero = True
        For Each rs In datos.Rows
            If primero Then
                primero = False
                strReturn = rs(0) & "¶" & rs(1) & "¶" & rs(2) & "¶" & rs(4) & "¶" & rs(5)
            Else
                strReturn = strReturn & "@" & rs(0) & "¶" & rs(1) & "¶" & rs(2) & "¶" & rs(4) & "¶" & rs(5)

            End If

        Next

        Return strReturn
    End Function

End Class
