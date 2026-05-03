Imports System.Xml
Imports System.IO
Public Class Form1

    Private con As SqlClient.SqlConnection

    Dim dtsDatos As New DataTable
    Dim pk As String
    Dim codigo As String
    Dim cliente As String
    Dim sucursal As String

    Dim DtpFdesde As Date
    Dim DtpFentrega As Date


    Dim fecha As String
    Dim fechaentrega As String
    Dim referencia As String
    Dim vendedor As String
    Dim contribuyente As String
    Dim tipolista As String

    Private Function PLCompletaCeros(ByVal VLValor As String, ByVal Ceros As Integer, ByVal bytComineza As Byte) As String
        Dim intLen As Integer
        Dim strReturn As String
        Dim intI As Integer
        intLen = (VLValor.Length)
        If intLen = Ceros Then
            Return VLValor
            Exit Function
        End If
        If intLen > Ceros Then
            strReturn = VLValor.Substring(0, Ceros)
            Return strReturn
            Exit Function
        End If

        strReturn = VLValor

        If bytComineza = 0 Then
            For intI = intLen To Ceros - 1
                strReturn = strReturn + "0"
            Next
        Else
            For intI = intLen To Ceros - 1
                strReturn = "0" + strReturn
            Next
        End If

        Return strReturn

    End Function

    Sub PLIniciaDatos()

        dtsDatos.Columns.Add("secuencia", GetType(String)).ReadOnly = True
        dtsDatos.Columns.Add("item", GetType(String)).ReadOnly = True
        dtsDatos.Columns.Add("um", GetType(String)).ReadOnly = True
        dtsDatos.Columns.Add("clase", GetType(String)).ReadOnly = True
        dtsDatos.Columns.Add("bodega", GetType(String)).ReadOnly = True
        dtsDatos.Columns.Add("cantidad", GetType(String)).ReadOnly = True
     

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim dtsFinal As New DataTable
        Dim negBuscarDatosPedidos As New negBuscarDatosPedidos

        Dim Ruta As String = Application.StartupPath & "\G30_000010_0000128_2011525_15237.xml"

        
        PLIniciaDatos()



        Try

            Dim m_xmld As XmlDocument

            Dim m_nodelist As XmlNodeList

            Dim m_node As XmlNode


          



            m_xmld = New XmlDocument()

            m_xmld.Load(Ruta)


            m_nodelist = m_xmld.SelectNodes("//cabecera")

            'Iniciamos el ciclo de lectura


            '<pk></pk>
            '<codigo></codigo>
            '<cliente></cliente>
            '<sucursal></sucursal>
            '<fecha></fecha>
            '<fechaentrega></fechaentrega>
            '<referencia></referencia>
            '<vendedor></vendedor>
            '<contribuyente></contribuyente>
            '<tipolista>A</tipolista>

            For Each m_node In m_nodelist

                pk = m_node.ChildNodes.Item(0).InnerText
                codigo = m_node.ChildNodes.Item(1).InnerText
                cliente = m_node.ChildNodes.Item(2).InnerText
                sucursal = m_node.ChildNodes.Item(3).InnerText



                DtpFdesde = m_node.ChildNodes.Item(4).InnerText()


                fecha = Trim(Str(DtpFdesde.Year)) + PLCompletaCeros(Me.DtpFdesde.Month, 2, 1) + PLCompletaCeros(Me.DtpFdesde.Day, 2, 1)

                DtpFentrega = m_node.ChildNodes.Item(5).InnerText()


                fechaentrega = Trim(Str(DtpFentrega.Year)) + PLCompletaCeros(Me.DtpFentrega.Month, 2, 1) + PLCompletaCeros(Me.DtpFentrega.Day, 2, 1)

                referencia = m_node.ChildNodes.Item(6).InnerText
                vendedor = m_node.ChildNodes.Item(7).InnerText
                contribuyente = m_node.ChildNodes.Item(8).InnerText
                tipolista = m_node.ChildNodes.Item(9).InnerText


            Next


            m_nodelist = m_xmld.SelectNodes("//detalle")

            'Iniciamos el ciclo de lectura

            '<secuencia></secuencia>
            '<item></item>
            '<um></um>
            '<clase></clase>
            '<bodega></bodega>
            '<cantidad></cantidad>

            For Each m_node In m_nodelist
                PLLlenarDate(m_node.ChildNodes.Item(0).InnerText, _
                            m_node.ChildNodes.Item(1).InnerText, _
                            m_node.ChildNodes.Item(2).InnerText, _
                            m_node.ChildNodes.Item(3).InnerText, _
                            m_node.ChildNodes.Item(4).InnerText, _
                            m_node.ChildNodes.Item(5).InnerText)

            Next

            negBuscarDatosPedidos.BaseDatos = "SysproCompanyF"

            con = negBuscarDatosPedidos.Conectar

            negBuscarDatosPedidos.DtsDatosDetalle = dtsDatos


            negBuscarDatosPedidos.pk = pk
            negBuscarDatosPedidos.codigo = codigo
            negBuscarDatosPedidos.cliente = cliente
            negBuscarDatosPedidos.sucursal = sucursal
            negBuscarDatosPedidos.referencia = referencia
            negBuscarDatosPedidos.vendedor = vendedor
            negBuscarDatosPedidos.contribuyente = contribuyente
            negBuscarDatosPedidos.fecha = fecha
            negBuscarDatosPedidos.fechaentrega = fechaentrega
            negBuscarDatosPedidos.tipolista = tipolista

            dtsFinal = negBuscarDatosPedidos.Buscar(con)

            GenerarXMLSORTOIL(dtsFinal)

            Me.dgrBusqueda.DataSource = dtsFinal

        Catch ex As Exception

            'Error trapping

            MessageBox.Show(ex.ToString())

        End Try

    End Sub

    Sub PLLlenarDate(ByVal secuencia As Integer, ByVal item As String, ByVal um As String, ByVal clase As String, ByVal bodega As String, ByVal cantidad As String)
        Dim dtrColumna As DataRow

        dtrColumna = Me.dtsDatos.NewRow()

        dtrColumna("secuencia") = secuencia
        dtrColumna("item") = item
        dtrColumna("um") = um
        dtrColumna("clase") = clase
        dtrColumna("bodega") = bodega

        dtrColumna("cantidad") = cantidad
      
        dtsDatos.Rows.Add(dtrColumna)

    End Sub



    Sub GenerarXMLSORTOIL(ByVal Datos As DataTable)

        Dim xdoc As New XmlDocument
        Dim tdoc As New XmlDocument

        Dim OrderDate As String
        Dim RequestedShipDate As String
        Dim CustRequestDate As String

        Dim DtmOrderDate As Date
        Dim DtmRequestedShipDate As Date
        Dim DtmCustRequestDate As Date


        Dim rs As DataRow

        If Datos.Rows.Count > 0 Then

            rs = Datos.Rows(0)



            DtmOrderDate = rs(3)
            DtmRequestedShipDate = rs(24)
            DtmCustRequestDate = rs(39)


            OrderDate = Trim(Str(DtmOrderDate.Year)) + "-" + PLCompletaCeros(DtmOrderDate.Month, 2, 1) + "-" + PLCompletaCeros(DtmOrderDate.Day, 2, 1)
            RequestedShipDate = Trim(Str(DtmRequestedShipDate.Year)) + "-" + PLCompletaCeros(DtmRequestedShipDate.Month, 2, 1) + "-" + PLCompletaCeros(DtmRequestedShipDate.Day, 2, 1)
            CustRequestDate = Trim(Str(DtmCustRequestDate.Year)) + "-" + PLCompletaCeros(DtmCustRequestDate.Month, 2, 1) + "-" + PLCompletaCeros(DtmCustRequestDate.Day, 2, 1)




            xdoc.Load(Application.StartupPath + "\SORTOIDOC.xml")
            tdoc.LoadXml(xdoc.OuterXml)

            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/CustomerPoNumber").InnerText = rs(0)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderActionType").InnerText = rs(1)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Customer").InnerText = rs(2)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderDate").InnerText = OrderDate
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/InvoiceTerms").InnerText = rs(4)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Currency").InnerText = rs(5)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShippingInstrs").InnerText = rs(6)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/CustomerName").InnerText = rs(7)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipAddress1").InnerText = rs(8)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipAddress2").InnerText = rs(9)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipAddress3").InnerText = rs(10)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipAddress4").InnerText = rs(11)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipAddress5").InnerText = rs(12)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/ShipPostalCode").InnerText = rs(13)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Email").InnerText = rs(14)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderDiscPercent1").InnerText = rs(15)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderDiscPercent2").InnerText = rs(16)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderDiscPercent3").InnerText = rs(17)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Warehouse").InnerText = rs(18)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/SalesOrder").InnerText = rs(19)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/OrderType").InnerText = rs(20)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Salesperson").InnerText = rs(21)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Branch").InnerText = rs(22)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/Area").InnerText = rs(23)
            tdoc.SelectSingleNode("SalesOrders/Orders/OrderHeader/RequestedShipDate").InnerText = RequestedShipDate

            Dim i As Integer
            i = 0
            For Each rsDetalle As DataRow In Datos.Rows
                If i = 0 Then

                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/CustomerPoLine").InnerText = rs(25)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/LineActionType").InnerText = rs(26)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/StockCode").InnerText = rs(27)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/StockDescription").InnerText = rs(28)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/Warehouse").InnerText = rs(29)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/OrderQty").InnerText = rs(30)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/OrderUom").InnerText = rs(31)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/Price").InnerText = rs(32)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/PriceUom").InnerText = rs(33)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/PriceCode").InnerText = rs(34)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/ProductClass").InnerText = rs(35)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/LineDiscPercent1").InnerText = rs(36)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/LineDiscPercent2").InnerText = rs(37)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/LineDiscPercent3").InnerText = rs(38)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/CustRequestDate").InnerText = CustRequestDate
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine/LineDiscValue").InnerText = rs(40)


                    i = 1
                Else

                    Dim nodo As XmlNode = tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails/StockLine").CloneNode(True)
                    nodo("CustomerPoLine").InnerText = rsDetalle(25)
                    nodo("LineActionType").InnerText = rsDetalle(26)
                    nodo("StockCode").InnerText = rsDetalle(27)
                    nodo("StockDescription").InnerText = rsDetalle(28)
                    nodo("Warehouse").InnerText = rsDetalle(29)
                    nodo("OrderQty").InnerText = rsDetalle(30)
                    nodo("OrderUom").InnerText = rsDetalle(31)
                    nodo("Price").InnerText = rsDetalle(32)
                    nodo("PriceUom").InnerText = rsDetalle(33)
                    nodo("PriceCode").InnerText = rsDetalle(34)
                    nodo("ProductClass").InnerText = rsDetalle(35)
                    nodo("LineDiscPercent1").InnerText = rsDetalle(36)
                    nodo("LineDiscPercent2").InnerText = rsDetalle(37)
                    nodo("LineDiscPercent3").InnerText = rsDetalle(38)
                    nodo("CustRequestDate").InnerText = CustRequestDate
                    nodo("LineDiscValue").InnerText = rsDetalle(40)
                    tdoc.SelectSingleNode("SalesOrders/Orders/OrderDetails").AppendChild(nodo)

                End If
            Next

            'Dim nodoAtri As XmlNode = tdoc.SelectSingleNode("factura/infoAdicional/campoAdicional")

            'If Not Directory.Exists(Ruta) Then
            '    Directory.CreateDirectory(Ruta)
            'End If
            'If Not Directory.Exists(Ruta + "\" + rs(2)) Then
            '    Directory.CreateDirectory(Ruta + "\" + rs(2))
            'End If
            'If Not Directory.Exists(Ruta + "\" + rs(2) + "\guiaRemision") Then
            '    Directory.CreateDirectory(Ruta + "\" + rs(2) + "\guiaRemision")
            'End If
            'If Not Directory.Exists(Ruta + "\" + rs(2) + "\guiaRemision" + "\" + Trim(Str(Now().Year)) + Trim(Str(Now.Month))) Then
            '    Directory.CreateDirectory(Ruta + "\" + rs(2) + "\guiaRemision" + "\" + Trim(Str(Now().Year)) + Trim(Str(Now.Month)))
            'End If



            'Ruta = Ruta + "\" + rs(2) + "\guiaRemision" + "\" + Trim(Str(Now().Year)) + Trim(Str(Now.Month))

            tdoc.Save("c:\SORTOIL.xml")
            tdoc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        End If

    End Sub


End Class
