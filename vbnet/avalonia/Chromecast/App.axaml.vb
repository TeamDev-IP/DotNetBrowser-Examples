﻿#Region "Copyright"

' Copyright © 2024, TeamDev. All rights reserved.
' 
' Redistribution and use in source and/or binary forms, with or without
' modification, must retain the above copyright notice and the following
' disclaimer.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#End Region

Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Markup.Xaml

Partial Public Class App
    Inherits Application

    Public Overrides Sub Initialize()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()
        Dim tempVar As Boolean = TypeOf ApplicationLifetime Is IClassicDesktopStyleApplicationLifetime
        Dim desktop As IClassicDesktopStyleApplicationLifetime = If(tempVar, CType(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime), Nothing)
        If tempVar Then
            desktop.MainWindow = New MainWindow()
        End If

        MyBase.OnFrameworkInitializationCompleted()
    End Sub
End Class