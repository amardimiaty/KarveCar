﻿Imports CustomControls
Public Class OrdenesExtrasVehiculo
    Public Function OrdenNombreExtras() As DataGridOrdenColumna
        Dim col As DataGridOrdenColumna
        col = New DataGridOrdenColumna
        With col
            .Name = "NOMBRE"
            .Criterio = DataGridOrdenColumna.euCriterio.Asc
            .AliasTabla = "EXVEH"
            .Campo = "NOMBRE"
        End With
        Return col
    End Function
End Class
