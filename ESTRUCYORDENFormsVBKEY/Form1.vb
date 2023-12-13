Imports System.Text

Public Class Form1
    Private data As New List(Of Integer)()
    Private pila As New Stack(Of Integer)()
    Private cola As New Queue(Of Integer)()
    Private lista As New List(Of Integer)()
    Private arbol As New BinaryTree()
    Private grafo As New Graph()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Agrega los algoritmos de ordenamiento al ComboBox
        cmbOrdenamientos.Items.Add("Bubble Sort")
        cmbOrdenamientos.Items.Add("Selection Sort")
        cmbOrdenamientos.Items.Add("Insertion Sort")
        cmbOrdenamientos.Items.Add("QuickSort")
        cmbOrdenamientos.Items.Add("MergeSort")
        cmbOrdenamientos.Items.Add("HeapSort")
        cmbOrdenamientos.Items.Add("ShellSort")
        cmbOrdenamientos.Items.Add("CountingSort")
        cmbOrdenamientos.Items.Add("RadixSort")
        cmbOrdenamientos.Items.Add("Salir")

        ' Selecciona el primer elemento por defecto
        cmbOrdenamientos.SelectedIndex = 0
    End Sub
    Public Class NodoArbol
        Public Valor As Integer
        Public Prioridad As String
        Public Izquierdo As NodoArbol
        Public Derecho As NodoArbol
    End Class

    Public Class BinaryTree
        Private raiz As NodoArbol

        Public Sub Insertar(valor As Integer, prioridad As String)
            raiz = InsertarRec(raiz, valor, prioridad)
        End Sub

        Private Function InsertarRec(nodo As NodoArbol, valor As Integer, prioridad As String) As NodoArbol
            If nodo Is Nothing Then
                nodo = New NodoArbol()
                nodo.Valor = valor
                nodo.Prioridad = prioridad
                nodo.Izquierdo = Nothing
                nodo.Derecho = Nothing
            ElseIf String.Compare(prioridad, nodo.Prioridad) < 0 OrElse (prioridad = nodo.Prioridad AndAlso valor < nodo.Valor) Then
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, valor, prioridad)
            Else
                nodo.Derecho = InsertarRec(nodo.Derecho, valor, prioridad)
            End If
            Return nodo
        End Function

        Public Sub Eliminar(valor As Integer, prioridad As String)
            raiz = EliminarRec(raiz, valor, prioridad)
        End Sub

        Private Function EliminarRec(nodo As NodoArbol, valor As Integer, prioridad As String) As NodoArbol
            If nodo Is Nothing Then
                Return nodo
            End If

            If valor < nodo.Valor Then
                nodo.Izquierdo = EliminarRec(nodo.Izquierdo, valor, prioridad)
            ElseIf valor > nodo.Valor Then
                nodo.Derecho = EliminarRec(nodo.Derecho, valor, prioridad)
            Else
                ' Nodo encontrado, realizar la eliminación
                If nodo.Izquierdo Is Nothing Then
                    Return nodo.Derecho
                ElseIf nodo.Derecho Is Nothing Then
                    Return nodo.Izquierdo
                End If

                ' Nodo con dos hijos: encontrar el sucesor inorden (el más grande en el subárbol izquierdo)
                nodo.Valor = EncontrarMaximo(nodo.Izquierdo)

                ' Eliminar el sucesor inorden
                nodo.Izquierdo = EliminarRec(nodo.Izquierdo, nodo.Valor, prioridad)
            End If

            Return nodo
        End Function

        Private Function EncontrarMaximo(nodo As NodoArbol) As Integer
            Dim maxValue As Integer = nodo.Valor
            While nodo.Derecho IsNot Nothing
                maxValue = nodo.Derecho.Valor
                nodo = nodo.Derecho
            End While
            Return maxValue
        End Function

        Public Sub RecorridoInorden(listBox As ListBox)
            RecorridoInordenRec(raiz, listBox)
        End Sub

        Private Sub RecorridoInordenRec(nodo As NodoArbol, listBox As ListBox)
            If nodo IsNot Nothing Then
                RecorridoInordenRec(nodo.Izquierdo, listBox)
                Dim nodeInfo As String = $"{nodo.Valor} ({nodo.Prioridad})"
                listBox.Items.Add(nodeInfo)
                RecorridoInordenRec(nodo.Derecho, listBox)
            End If
        End Sub

        Public Function Buscar(valor As Integer) As Boolean
            Return BuscarRec(raiz, valor)
        End Function

        Private Function BuscarRec(nodo As NodoArbol, valor As Integer) As Boolean
            If nodo Is Nothing Then
                Return False
            End If

            If valor = nodo.Valor Then
                Return True
            End If

            If valor < nodo.Valor Then
                Return BuscarRec(nodo.Izquierdo, valor)
            Else
                Return BuscarRec(nodo.Derecho, valor)
            End If
        End Function
    End Class


    Public Class Graph
        Private adjacencyList As Dictionary(Of Integer, List(Of Integer))

        Public Sub New()
            adjacencyList = New Dictionary(Of Integer, List(Of Integer))()
        End Sub

        Public Sub AddVertex(vertex As Integer)
            If Not adjacencyList.ContainsKey(vertex) Then
                adjacencyList(vertex) = New List(Of Integer)()
            Else
                MessageBox.Show("El vértice ya existe en el grafo.")
            End If
        End Sub


        Public Function ContainsVertex(vertex As Integer) As Boolean
            Return adjacencyList.ContainsKey(vertex)
        End Function

        Public Sub RemoveVertex(vertex As Integer)
            adjacencyList.Remove(vertex)

            ' Eliminar el vértice de las listas de adyacencia de otros vértices
            For Each adjList In adjacencyList.Values
                adjList.Remove(vertex)
            Next
        End Sub

        Public Sub AddEdge(startVertex As Integer, endVertex As Integer)
            If adjacencyList.ContainsKey(startVertex) AndAlso adjacencyList.ContainsKey(endVertex) Then
                adjacencyList(startVertex).Add(endVertex)
                adjacencyList(endVertex).Add(startVertex) ' Suponiendo un grafo no dirigido
            End If
        End Sub

        Public Sub RemoveEdge(startVertex As Integer, endVertex As Integer)
            If adjacencyList.ContainsKey(startVertex) AndAlso adjacencyList.ContainsKey(endVertex) Then
                adjacencyList(startVertex).Remove(endVertex)
                adjacencyList(endVertex).Remove(startVertex)
            End If
        End Sub

        Public Function GetVertices() As List(Of Integer)
            Return New List(Of Integer)(adjacencyList.Keys)
        End Function

        Public Function GetEdges() As List(Of String)
            Dim edgesSet As New HashSet(Of String)()

            For Each vertex In adjacencyList.Keys
                For Each neighbor In adjacencyList(vertex)
                    ' Ordenar los vértices para asegurar una representación única
                    Dim edge As String = $"{Math.Min(vertex, neighbor)}-{Math.Max(vertex, neighbor)}"
                    edgesSet.Add(edge)
                Next
            Next

            Return edgesSet.ToList()
        End Function

        Public Sub Clear()
            adjacencyList.Clear()
        End Sub

        Public Overrides Function ToString() As String
            Dim result As New StringBuilder()
            For Each vertex In adjacencyList.Keys
                result.Append($"{vertex}: ")
                result.Append(String.Join(", ", adjacencyList(vertex)))
                result.AppendLine()
            Next
            Return result.ToString()
        End Function
    End Class


    Private Sub btnAgregarArbol_Click(sender As Object, e As EventArgs) Handles btnAgregarArbol.Click
        Dim num As Integer ' Declaración de la variable num
        Dim entrada As String() = txtArbol.Text.Split(" "c)

        If entrada.Length = 2 AndAlso Integer.TryParse(entrada(0), num) Then
            Dim prioridad As String = entrada(1).ToLower() ' Tomar la segunda parte como prioridad en minúsculas
            If EsPrioridadValida(prioridad) Then
                arbol.Insertar(num, prioridad)
                UpdateArbolListBox()
            Else
                MessageBox.Show("Prioridad no válida. Se asignará 'media' por defecto.")
                arbol.Insertar(num, "media")
                UpdateArbolListBox()
            End If
        Else
            MessageBox.Show("Formato de entrada incorrecto. Por favor, ingrese un número y su prioridad (por ejemplo, '3 alta').")
        End If

    End Sub

    Private Sub btnEliminarArbol_Click(sender As Object, e As EventArgs) Handles btnEliminarArbol.Click
        Dim num As Integer ' Declaración de la variable num

        If listBoxArbol.SelectedIndex <> -1 Then
            ' Se ha seleccionado un elemento en la ListBox
            Dim selectedItem As String = listBoxArbol.SelectedItem.ToString()
            Dim parts As String() = selectedItem.Split(" "c)

            If parts.Length = 2 AndAlso Integer.TryParse(parts(0), num) Then
                Dim priority As String = parts(1).Trim("("c, ")"c).ToLower()

                arbol.Eliminar(num, priority)
                UpdateArbolListBox()
            Else
                MessageBox.Show("No se pudo determinar el valor y la prioridad del elemento seleccionado.")
            End If
        Else
            MessageBox.Show("Por favor, seleccione un elemento para eliminar.")
        End If
    End Sub
    Private Sub UpdateArbolListBox()
        listBoxArbol.Items.Clear()
        arbol.RecorridoInorden(listBoxArbol)
    End Sub
    Private Function EsPrioridadValida(prioridad As String) As Boolean
        Return prioridad = "alta" OrElse prioridad = "media" OrElse prioridad = "baja"
    End Function

    Private Sub TraverseAndAddToList(nodo As NodoArbol, items As ListBox.ObjectCollection)
        If nodo IsNot Nothing Then
            TraverseAndAddToList(nodo.Izquierdo, items)
            Dim nodeInfo As String = $"{nodo.Valor} ({nodo.Prioridad})"
            items.Add(nodeInfo)
            TraverseAndAddToList(nodo.Derecho, items)
        End If
    End Sub


    Private Sub btnEjecutarOrdenamientos_Click(sender As Object, e As EventArgs) Handles btnEjecutarOrdenamientos.Click
        Dim selectedSort As Integer = cmbOrdenamientos.SelectedIndex

        ' Ejecutar el algoritmo de ordenamiento correspondiente
        Select Case selectedSort
            Case 0
                BubbleSort(data)
                Exit Select
            Case 1
                SelectionSort(data)
                Exit Select
            Case 2
                InsertionSort(data)
                Exit Select
            Case 3
                QuickSort(data, 0, data.Count - 1)
                Exit Select
            Case 4
                MergeSort(data, 0, data.Count - 1)
                Exit Select
            Case 5
                HeapSort(data)
                Exit Select
            Case 6
                ShellSort(data)
                Exit Select
            Case 7
                CountingSort(data)
                Exit Select
            Case 8
                RadixSort(data)
                Exit Select
                ' Añade más casos según sea necesario para otros algoritmos
        End Select

        ' Mostrar los datos ordenados
        MessageBox.Show("Datos ordenados: " & String.Join(", ", data))
    End Sub
    Private Sub BubbleSort(data As List(Of Integer))
        For i As Integer = 0 To data.Count - 1
            For j As Integer = 0 To data.Count - i - 1
                If j + 1 < data.Count AndAlso data(j) > data(j + 1) Then
                    ' Intercambiar elementos si están en el orden incorrecto
                    Dim temp As Integer = data(j)
                    data(j) = data(j + 1)
                    data(j + 1) = temp
                End If
            Next
        Next

        Console.WriteLine("Datos ordenados con Bubble Sort.")
    End Sub
    Private Sub SelectionSort(data As List(Of Integer))
        For i As Integer = 0 To data.Count - 1
            Dim minIndex As Integer = i

            ' Encontrar el índice del elemento mínimo en el resto del array
            For j As Integer = i + 1 To data.Count - 1
                If data(j) < data(minIndex) Then
                    minIndex = j
                End If
            Next

            ' Intercambiar el elemento mínimo con el primer elemento sin ordenar
            Dim temp As Integer = data(minIndex)
            data(minIndex) = data(i)
            data(i) = temp
        Next

        Console.WriteLine("Datos ordenados con Selection Sort.")
    End Sub
    Private Sub InsertionSort(data As List(Of Integer))
        For i As Integer = 1 To data.Count - 1
            Dim key As Integer = data(i)
            Dim j As Integer = i - 1

            ' Mover los elementos del array que son mayores que key a una posición adelante de su posición actual
            While j >= 0 AndAlso data(j) > key
                data(j + 1) = data(j)
                j -= 1
            End While

            ' Insertar el elemento key en su posición correcta
            data(j + 1) = key
        Next

        Console.WriteLine("Datos ordenados con Insertion Sort.")
    End Sub
    Private Sub QuickSort(data As List(Of Integer), low As Integer, high As Integer)
        If low < high Then
            Dim partitionIndex As Integer = Partition(data, low, high)

            QuickSort(data, low, partitionIndex - 1)
            QuickSort(data, partitionIndex + 1, high)
        End If
    End Sub
    Private Function Partition(data As List(Of Integer), low As Integer, high As Integer) As Integer
        Dim pivot As Integer = data(high)
        Dim i As Integer = low - 1

        For j As Integer = low To high - 1
            If data(j) < pivot Then
                i += 1
                Swap(data, i, j)
            End If
        Next

        Swap(data, i + 1, high)
        Return i + 1
    End Function
    Private Sub Merge(data As List(Of Integer), left As Integer, middle As Integer, right As Integer)
        Dim n1 As Integer = middle - left + 1
        Dim n2 As Integer = right - middle

        Dim LeftArray(n1 - 1) As Integer
        Dim RightArray(n2 - 1) As Integer

        Dim i As Integer, j As Integer

        ' Copiar datos a los subarreglos LeftArray y RightArray
        For i = 0 To n1 - 1
            LeftArray(i) = data(left + i)
        Next

        For j = 0 To n2 - 1
            RightArray(j) = data(middle + 1 + j)
        Next

        ' Fusionar los subarreglos de nuevo en el arreglo original
        i = 0
        j = 0
        Dim k As Integer = left

        While i < n1 AndAlso j < n2
            If LeftArray(i) <= RightArray(j) Then
                data(k) = LeftArray(i)
                i += 1
            Else
                data(k) = RightArray(j)
                j += 1
            End If
            k += 1
        End While

        ' Copiar los elementos restantes de LeftArray (si los hay)
        While i < n1
            data(k) = LeftArray(i)
            i += 1
            k += 1
        End While

        ' Copiar los elementos restantes de RightArray (si los hay)
        While j < n2
            data(k) = RightArray(j)
            j += 1
            k += 1
        End While
    End Sub

    Private Sub MergeSort(data As List(Of Integer), left As Integer, right As Integer)
        If left < right Then
            Dim middle As Integer = (left + right) \ 2

            MergeSort(data, left, middle)
            MergeSort(data, middle + 1, right)

            Merge(data, left, middle, right)
        End If
    End Sub

    Private Sub Swap(data As List(Of Integer), index1 As Integer, index2 As Integer)
        Dim temp As Integer = data(index1)
        data(index1) = data(index2)
        data(index2) = temp
    End Sub
    Private Sub HeapSort(data As List(Of Integer))
        Dim n As Integer = data.Count

        For i As Integer = n \ 2 - 1 To 0 Step -1
            Heapify(data, n, i)
        Next

        For i As Integer = n - 1 To 1 Step -1
            Dim temp As Integer = data(0)
            data(0) = data(i)
            data(i) = temp

            Heapify(data, i, 0)
        Next
    End Sub

    Private Sub Heapify(data As List(Of Integer), n As Integer, i As Integer)
        Dim largest As Integer = i
        Dim left As Integer = 2 * i + 1
        Dim right As Integer = 2 * i + 2

        If left < n AndAlso data(left) > data(largest) Then
            largest = left
        End If

        If right < n AndAlso data(right) > data(largest) Then
            largest = right
        End If

        If largest <> i Then
            Dim swap As Integer = data(i)
            data(i) = data(largest)
            data(largest) = swap

            Heapify(data, n, largest)
        End If
    End Sub
    Private Sub ShellSort(data As List(Of Integer))
        Dim n As Integer = data.Count

        For gap As Integer = n \ 2 To 1 Step gap \ 2
            For i As Integer = gap To n - 1
                Dim temp As Integer = data(i)
                Dim j As Integer = i

                While j >= gap AndAlso data(j - gap) > temp
                    data(j) = data(j - gap)
                    j -= gap
                End While

                data(j) = temp
            Next
        Next
    End Sub

    Private Sub CountingSort(data As List(Of Integer))
        Dim n As Integer = data.Count
        Dim output As Integer() = New Integer(n - 1) {}

        Dim max As Integer = data.Max()
        Dim min As Integer = data.Min()
        Dim range As Integer = max - min + 1

        Dim count As Integer() = New Integer(range - 1) {}
        Dim outputData As Integer() = New Integer(n - 1) {}

        For i As Integer = 0 To n - 1
            count(data(i) - min) += 1
        Next

        For i As Integer = 1 To range - 1
            count(i) += count(i - 1)
        Next

        For i As Integer = n - 1 To 0 Step -1
            output(count(data(i) - min) - 1) = data(i)
            count(data(i) - min) -= 1
        Next

        For i As Integer = 0 To n - 1
            data(i) = output(i)
        Next
    End Sub

    Private Sub RadixSort(data As List(Of Integer))
        Dim max As Integer = data.Max()

        ' Asegurarse de que exp sea menor o igual a max
        For exp As Integer = 1 To max
            CountingSortRadix(data, exp)
        Next
    End Sub


    Private Sub CountingSortRadix(data As List(Of Integer), exp As Integer)
        Dim n As Integer = data.Count
        Dim output As Integer() = New Integer(n - 1) {}
        Dim count As Integer() = New Integer(9) {}

        For i As Integer = 0 To n - 1
            count((data(i) / exp) Mod 10) += 1
        Next

        For i As Integer = 1 To 9
            count(i) += count(i - 1)
        Next

        For i As Integer = n - 1 To 0 Step -1
            output(count((data(i) / exp) Mod 10) - 1) = data(i)
            count((data(i) / exp) Mod 10) -= 1
        Next

        For i As Integer = 0 To n - 1
            data(i) = output(i)
        Next
    End Sub
    Private Sub btnAgregarDatos_Click(sender As Object, e As EventArgs) Handles btnAgregarDatos.Click
        ' Puedes validar la entrada del usuario aquí
        Dim input As String = txtDatos.Text
        Dim elementos As String() = input.Split(" "c)

        For Each elemento As String In elementos
            Dim num As Integer
            If Integer.TryParse(elemento, num) Then
                data.Add(num)
            End If
        Next

        cmbOrdenamientos.Refresh()

        MessageBox.Show("Datos agregados correctamente.")

    End Sub
    Private Sub btnLIMPIAR_Click(sender As Object, e As EventArgs) Handles btnLIMPIAR.Click
        data.Clear()
        txtDatos.Clear()
        pila.Clear()
        cola.Clear()
        lista.Clear()
        'arbol.Clear()
        grafo.Clear()

        MessageBox.Show("Datos limpiados.")
    End Sub

    Private Sub btnAgregarPila_Click(sender As Object, e As EventArgs) Handles btnAgregarPila.Click
        ' Agregar un elemento a la pila
        Dim elemento As Integer
        If Integer.TryParse(txtElementoPila.Text, elemento) Then
            pila.Push(elemento)
            UpdatePilaListBox()
        Else
            MessageBox.Show("Ingrese un número válido.")
        End If

    End Sub

    Private Sub btnEliminarPila_Click(sender As Object, e As EventArgs) Handles btnEliminarPila.Click
        ' Eliminar un elemento de la pila
        If pila.Count > 0 Then
            pila.Pop()
            UpdatePilaListBox()
        Else
            MessageBox.Show("La pila está vacía.")
        End If

    End Sub
    Private Sub UpdatePilaListBox()
        ' Actualizar la ListBox con los elementos de la pila
        ListBox1.Items.Clear()
        For Each elemento As Integer In pila
            ListBox1.Items.Add(elemento)
        Next
    End Sub

    Private Sub btnAgregarCola_Click(sender As Object, e As EventArgs) Handles btnAgregarCola.Click
        Dim elemento As Integer

        If Integer.TryParse(txtElementoCola.Text, elemento) Then
            cola.Enqueue(elemento)
            UpdateColaListBox()
        Else
            MessageBox.Show("Ingrese un número válido.")
        End If

    End Sub

    Private Sub btnEliminarCola_Click(sender As Object, e As EventArgs) Handles btnEliminarCola.Click
        If cola.Count > 0 Then
            cola.Dequeue()
            UpdateColaListBox()
        Else
            MessageBox.Show("La cola está vacía.")
        End If

    End Sub
    Private Sub UpdateColaListBox()
        ' Actualizar la ListBox con los elementos de la cola
        listBoxCola.Items.Clear()
        For Each elemento As Integer In cola
            listBoxCola.Items.Add(elemento)
        Next
    End Sub

    Private Sub btnAgregarLista_Click(sender As Object, e As EventArgs) Handles btnAgregarLista.Click
        Dim elemento As Integer

        If Integer.TryParse(txtElementoLista.Text, elemento) Then
            lista.Add(elemento)
            UpdateListaListBox()
        Else
            MessageBox.Show("Ingrese un número válido.")
        End If

    End Sub

    Private Sub btnEliminarLista_Click(sender As Object, e As EventArgs) Handles btnEliminarLista.Click
        If lista.Count > 0 Then
            lista.RemoveAt(lista.Count - 1)
            UpdateListaListBox()
        Else
            MessageBox.Show("La lista está vacía.")
        End If


    End Sub
    Private Sub UpdateListaListBox()
        ' Actualizar la ListBox con los elementos de la lista
        listBoxLista.Items.Clear()

        For Each elemento As Integer In lista
            listBoxLista.Items.Add(elemento)
        Next
    End Sub
    Private Sub btnAgregarVerticeGrafo_Click(sender As Object, e As EventArgs) Handles btnAgregarVerticeGrafo.Click
        Dim vertice As Integer

        If Integer.TryParse(txtVerticeGrafo.Text, vertice) Then
            grafo.AddVertex(vertice)
            UpdateGrafoListBox()
        Else
            MessageBox.Show("Ingrese un número válido para el vértice.")
        End If
    End Sub

    Private Sub btnEliminarVerticeGrafo_Click(sender As Object, e As EventArgs) Handles btnEliminarVerticeGrafo.Click
        If listBoxGrafos.SelectedIndex <> -1 Then
            Dim vertice As Integer = DirectCast(listBoxGrafos.SelectedItem, Integer)
            grafo.RemoveVertex(vertice)
            UpdateGrafoListBox()
        Else
            MessageBox.Show("Seleccione un vértice para eliminar.")
        End If
    End Sub
    Private Sub btnAgregarAristaGrafo_Click(sender As Object, e As EventArgs) Handles btnAgregarAristaGrafo.Click
        ' Verificar si los datos ingresados son números válidos
        Dim inicio As Integer
        Dim fin As Integer

        If Integer.TryParse(txtInicioArista.Text, inicio) AndAlso Integer.TryParse(txtFinArista.Text, fin) Then
            ' Verificar si los vértices existen en el grafo
            If grafo.ContainsVertex(inicio) AndAlso grafo.ContainsVertex(fin) Then
                ' Agregar la arista
                grafo.AddEdge(inicio, fin)
                UpdateGrafoListBox()
            Else
                MessageBox.Show("Ingrese vértices válidos existentes en el grafo.")
            End If
        Else
            MessageBox.Show("Ingrese números válidos para los vértices de la arista.")
        End If
    End Sub

    Private Sub btnEliminarAristaGrafo_Click(sender As Object, e As EventArgs) Handles btnEliminarAristaGrafo.Click
        If listBoxAristas.SelectedIndex <> -1 Then
            Dim arista As String = listBoxAristas.SelectedItem.ToString()
            Dim vertices As String() = arista.Split("-"c)
            Dim inicio As Integer = Integer.Parse(vertices(0))
            Dim fin As Integer = Integer.Parse(vertices(1))
            grafo.RemoveEdge(inicio, fin)
            UpdateGrafoListBox()
        Else
            MessageBox.Show("Seleccione una arista para eliminar.")
        End If
    End Sub

    Private Sub UpdateGrafoListBox()
        listBoxGrafos.Items.Clear()
        For Each vertex As Integer In grafo.GetVertices()
            listBoxGrafos.Items.Add(vertex)
        Next

        listBoxAristas.Items.Clear()
        For Each edge As String In grafo.GetEdges()
            listBoxAristas.Items.Add(edge)
        Next
    End Sub
    Private WithEvents btnEliminarVerticeGrafo As Button
    Private WithEvents btnAgregarVerticeGrafo As Button

    Public Sub New()
        ' Llamado por el diseñador de Windows Forms.
        InitializeComponent()

        ' Inicializar el botón y establecer sus propiedades
        btnEliminarVerticeGrafo = New Button()
        btnEliminarVerticeGrafo.Text = "Eliminar Vértice"
        btnEliminarVerticeGrafo.Location = New Point(550, 480) ' Establecer las coordenadas X y Y
        btnEliminarVerticeGrafo.Size = New Size(80, 60)
        ' Otras propiedades según sea necesario...

        ' Agregar el botón al formulario
        Me.Controls.Add(btnEliminarVerticeGrafo)
        btnAgregarVerticeGrafo = New Button()
        btnAgregarVerticeGrafo.Text = "Agregar Vértice"
        btnAgregarVerticeGrafo.Location = New Point(550, 400) ' Establecer las coordenadas X y Y
        btnAgregarVerticeGrafo.Size = New Size(80, 60)
        ' Otras propiedades según sea necesario...

        ' Agregar el botón al formulario
        Me.Controls.Add(btnAgregarVerticeGrafo)
    End Sub
End Class
