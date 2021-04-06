Imports MySql.Data.MySqlClient

Public Class Form1

    'server=localhost; user=yout_database_user; password=your_database_password; database=your_database_name
    Dim Connection As New MySqlConnection("server=localhost; user=root; password=; database=rfid_user")
    Dim MySQLCMD As New MySqlCommand
    Dim MySQLDA As New MySqlDataAdapter
    Dim DT As New DataTable
    Dim Table_Name As String = "etudiants" 'nom de la table
    Dim Data As Integer

    Dim LoadImagesStr As Boolean = False
    Dim IDRam As String
    Dim IMG_FileNameInput As String
    Dim StatusInput As String = "Save"
    Dim SqlCmdSearchstr As String

    Public Shared StrSerialIn As String
    Dim GetID As Boolean = False
    Dim ViewUserData As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        PanelRegistrationandEditUser.Visible = False
        PanelUserData.Visible = False
        PanelConnection.Visible = True
        ComboBoxBaudRate.SelectedIndex = 3
    End Sub

    Private Sub ShowData()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Echec de la Connection!!!" & vbCrLf & "Vérifier l'état du serveur !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If LoadImagesStr = False Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT nom, id, codePermanent, niveau, ufr FROM  etudiants  ORDER BY nom"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    DataGridView1.DataSource = Nothing
                    DataGridView1.DataSource = DT
                    DataGridView1.Columns(2).DefaultCellStyle.Format = "c"
                    DataGridView1.DefaultCellStyle.ForeColor = Color.Black
                    DataGridView1.ClearSelection()
                Else
                    DataGridView1.DataSource = DT
                End If
            Else
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT image FROM etudiants WHERE id LIKE '" & IDRam & "'"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                    Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                    PictureBoxImagePreview.Image = Image.FromStream(lmgStr)
                    PictureBoxImagePreview.SizeMode = PictureBoxSizeMode.StretchImage
                    lmgStr.Close()
                End If
                LoadImagesStr = False
            End If
        Catch ex As Exception
            MsgBox("Echec du chargement de la base de données!!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreur")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub

    Private Sub ShowDataUser()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Echec de la connection!!!" & vbCrLf & "Vérifier l'état du serveur!!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLCMD.CommandType = CommandType.Text
            MySQLCMD.CommandText = "SELECT * FROM etudiants  WHERE id LIKE '" & LabelID.Text.Substring(5, LabelID.Text.Length - 5) & "'"
            MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                PictureBoxImage.Image = Image.FromStream(lmgStr)
                lmgStr.Close()

                LabelID.Text = "ID : " & DT.Rows(0).Item("id")
                LabelNom.Text = DT.Rows(0).Item("nom")
                LabelCodePermanent.Text = DT.Rows(0).Item("codePermanent")
                LabelNiveau.Text = DT.Rows(0).Item("niveau")
                ' LabelUFR.Text = DT.Rows(0).Item("ufr")
            Else
                MsgBox("Carte introuvable !!!" & vbCr & "Veuillez inscrire cette carte s'il vous plait !!.", MsgBoxStyle.Information, "Message d'information")
            End If
        Catch ex As Exception
            MsgBox("Echec de la connection à la base de données !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreur")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub

    Private Sub ClearInputUpdateData()
        TextBoxNom.Text = ""
        LabelGetID.Text = "________"
        TextBoxPrenom.Text = ""
        TextBoxCodePermanent.Text = ""
        TextBoxSolde.Text = ""
        PictureBoxImageInput.Image = My.Resources.icone_click
    End Sub

    Private Sub ButtonConnection_Click(sender As Object, e As EventArgs) Handles ButtonConnection.Click
        PictureBoxSelect.Top = ButtonConnection.Top
        PanelUserData.Visible = False
        PanelRegistrationandEditUser.Visible = False
        PanelConnection.Visible = True
    End Sub

    Private Sub ButtonUserData_Click(sender As Object, e As EventArgs) Handles ButtonUserData.Click
        If TimerSerialIn.Enabled = False Then
            MsgBox("Impossible de lire les données !!!" & vbCr & "Cliquer sur le menu de Connection ensuite le bouton de Connection.", MsgBoxStyle.Information, "Information")
            Return
        Else
            StrSerialIn = ""
            ViewUserData = True
            PictureBoxSelect.Top = ButtonUserData.Top
            PanelRegistrationandEditUser.Visible = False
            PanelConnection.Visible = False
            PanelUserData.Visible = True
        End If
    End Sub

    Private Sub ButtonRegistration_Click(sender As Object, e As EventArgs) Handles ButtonRegistration.Click
        StrSerialIn = ""
        ViewUserData = False
        PictureBoxSelect.Top = ButtonRegistration.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelRegistrationandEditUser.Visible = True
        ShowData()
    End Sub

    Private Sub PanelConnection_Paint(sender As Object, e As PaintEventArgs) Handles PanelConnection.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelConnection.ClientRectangle)
    End Sub

    Private Sub PanelConnection_Resize(sender As Object, e As EventArgs) Handles PanelConnection.Resize
        PanelConnection.Invalidate()
    End Sub

    Private Sub PanelUserData_Paint(sender As Object, e As PaintEventArgs) Handles PanelUserData.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelUserData.ClientRectangle)
    End Sub

    Private Sub PanelUserData_Resize(sender As Object, e As EventArgs) Handles PanelUserData.Resize
        PanelUserData.Invalidate()
    End Sub

    Private Sub PanelRegistrationandEditUser_Paint(sender As Object, e As PaintEventArgs) Handles PanelRegistrationandEditUser.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelRegistrationandEditUserData.ClientRectangle)
    End Sub

    Private Sub PanelRegistrationandEditUser_Resize(sender As Object, e As EventArgs) Handles PanelRegistrationandEditUser.Resize
        PanelRegistrationandEditUser.Invalidate()
    End Sub

    Private Sub ButtonScanPort_Click(sender As Object, e As EventArgs) Handles ButtonScanPort.Click
        ComboBoxPort.Items.Clear()
        Dim myPort As Array
        Dim i As Integer
        myPort = IO.Ports.SerialPort.GetPortNames()
        ComboBoxPort.Items.AddRange(myPort)
        i = ComboBoxPort.Items.Count
        i = i - i
        Try
            ComboBoxPort.SelectedIndex = i
        Catch ex As Exception
            MsgBox("Port COM non détecté", MsgBoxStyle.Critical, "Message d'erreur")
            ComboBoxPort.Text = ""
            ComboBoxPort.Items.Clear()
            Return
        End Try
        ComboBoxPort.DroppedDown = True
    End Sub

    Private Sub ButtonScanPort_MouseHover(sender As Object, e As EventArgs) Handles ButtonScanPort.MouseHover
        ButtonScanPort.ForeColor = Color.White
    End Sub

    Private Sub ButtonScanPort_MouseLeave(sender As Object, e As EventArgs) Handles ButtonScanPort.MouseLeave
        ButtonScanPort.ForeColor = Color.FromArgb(255, 128, 128)
    End Sub

    Private Sub ButtonConnect_Click(sender As Object, e As EventArgs) Handles ButtonConnect.Click
        If ButtonConnect.Text = "Connecter" Then
            SerialPort1.BaudRate = ComboBoxBaudRate.SelectedItem
            SerialPort1.PortName = ComboBoxPort.SelectedItem
            Try
                SerialPort1.Open()
                TimerSerialIn.Start()
                ButtonConnect.Text = "Déconnecter"
                PictureBoxStatusConnect.Image = My.Resources.icone_connected
            Catch ex As Exception
                MsgBox("Echec de la connection!!!" & vbCr & "L'Arduino n'est pas détecté.", MsgBoxStyle.Critical, "Message d'erreur")
                PictureBoxStatusConnect.Image = My.Resources.Disconnect
            End Try
        ElseIf ButtonConnect.Text = "Déconnecté" Then
            PictureBoxStatusConnect.Image = My.Resources.Disconnect
            ButtonConnect.Text = "Connecté"
            LabelConnectionStatus.Text = "Statut de la connection : Déconnecté"
            TimerSerialIn.Stop()
            SerialPort1.Close()
        End If
    End Sub

    Private Sub ButtonConnect_MouseHover(sender As Object, e As EventArgs) Handles ButtonConnect.MouseHover
        ButtonConnect.ForeColor = Color.White
    End Sub

    Private Sub ButtonConnect_MouseLeave(sender As Object, e As EventArgs) Handles ButtonConnect.MouseLeave
        ButtonConnect.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        LabelID.Text = "ID : ________"
        LabelNom.Text = "En cours..."
        LabelCodePermanent.Text = "En cours..."
        LabelNiveau.Text = "En cours..."
        ' LabelUFR.Text = "En cours..."
        PictureBoxImage.Image = Nothing
    End Sub

    Private Sub ButtonClear_MouseHover(sender As Object, e As EventArgs) Handles ButtonClear.MouseHover
        ButtonClear.ForeColor = Color.White
    End Sub

    Private Sub ButtonClear_MouseLeave(sender As Object, e As EventArgs) Handles ButtonClear.MouseLeave
        ButtonClear.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Dim mstream As New System.IO.MemoryStream()
        Dim arrImage() As Byte

        If TextBoxNom.Text = "" Then
            MessageBox.Show("Nom ne peut pas etre vide !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxPrenom.Text = "" Then
            MessageBox.Show("Code Permanent ne peut pas etre vide !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxCodePermanent.Text = "" Then
            MessageBox.Show("Niveau ne peut pas etre vide !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxSolde.Text = "" Then
            MessageBox.Show("UFR ne peut pas etre vide !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If StatusInput = "Save" Then
            If IMG_FileNameInput <> "" Then
                PictureBoxImageInput.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                arrImage = mstream.GetBuffer()
            Else
                MessageBox.Show("Veuillez sélectioner une image !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Try
                Connection.Open()
            Catch ex As Exception
                MessageBox.Show("Echec de conenction !!!" & vbCrLf & "Vérifier l'état de votre serveur !!!", "Messag d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            Try
                MySQLCMD = New MySqlCommand
                With MySQLCMD
                    .CommandText = "INSERT INTO etudiants(nom, id, codePermanent, niveau, ufr, image) VALUES (@nom, @id, @codePermanent, @niveau, @ufr, @image)"
                    .Connection = Connection
                    .Parameters.AddWithValue("@nom", TextBoxNom.Text)
                    .Parameters.AddWithValue("@id", LabelGetID.Text)
                    .Parameters.AddWithValue("@codePermanent", TextBoxPrenom.Text)
                    .Parameters.AddWithValue("@niveau", TextBoxCodePermanent.Text)
                    .Parameters.AddWithValue("@ufr", TextBoxSolde.Text)
                    .Parameters.AddWithValue("@image", arrImage)
                    .ExecuteNonQuery()
                End With
                MsgBox("Informations enregistrées avec succés", MsgBoxStyle.Information, "Information")
                IMG_FileNameInput = ""
                ClearInputUpdateData()
            Catch ex As Exception
                MsgBox("Echec d'enregistrement des données !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreur")
                Connection.Close()
                Return
            End Try
            Connection.Close()

        Else

            If IMG_FileNameInput <> "" Then
                PictureBoxImageInput.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                arrImage = mstream.GetBuffer()

                Try
                    Connection.Open()
                Catch ex As Exception
                    MessageBox.Show("Echec de la Connection !!!" & vbCrLf & "Vérifier l'état du serveur !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                Try
                    MySQLCMD = New MySqlCommand
                    With MySQLCMD
                        .CommandText = "UPDATE etudiants  SET  nom=@nom,id=@id,codePermanent=@codePermanent,niveau=@niveau,ufr=@ufr,image=@image WHERE id=@id "
                        .Connection = Connection
                        .Parameters.AddWithValue("@nom", TextBoxNom.Text)
                        .Parameters.AddWithValue("@id", LabelGetID.Text)
                        .Parameters.AddWithValue("@codePermanent", TextBoxPrenom.Text)
                        .Parameters.AddWithValue("@niveau", TextBoxCodePermanent.Text)
                        .Parameters.AddWithValue("@ufr", TextBoxSolde.Text)
                        .Parameters.AddWithValue("@image", arrImage)
                        .ExecuteNonQuery()
                    End With
                    MsgBox("Données modifiées avec succés", MsgBoxStyle.Information, "Information")
                    IMG_FileNameInput = ""
                    ButtonSave.Text = "Enregistrer"
                    ClearInputUpdateData()
                Catch ex As Exception
                    MsgBox("Echec lors de la modification des informations !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreur")
                    Connection.Close()
                    Return
                End Try
                Connection.Close()

            Else

                Try
                    Connection.Open()
                Catch ex As Exception
                    MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                Try
                    MySQLCMD = New MySqlCommand
                    With MySQLCMD
                        .CommandText = "UPDATE etudiants SET  nom=@nom,id=@id,codePermanent=@codePermanent,niveau=@niveau,ufr=@ufr WHERE id=@id "
                        .Connection = Connection
                        .Parameters.AddWithValue("@nom", TextBoxNom.Text)
                        .Parameters.AddWithValue("@id", LabelGetID.Text)
                        .Parameters.AddWithValue("@codePermanent", TextBoxPrenom.Text)
                        .Parameters.AddWithValue("@niveau", TextBoxCodePermanent.Text)
                        .Parameters.AddWithValue("@ufr", TextBoxSolde.Text)
                        .ExecuteNonQuery()
                    End With
                    MsgBox("Modication effectuée avec succés", MsgBoxStyle.Information, "Information")
                    ButtonSave.Text = "Enregistrer"
                    ClearInputUpdateData()
                Catch ex As Exception
                    MsgBox("Echec lors de la modification des donénes!!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreur")
                    Connection.Close()
                    Return
                End Try
                Connection.Close()
            End If
            StatusInput = "Save"
        End If
        PictureBoxImagePreview.Image = Nothing
        ShowData()
    End Sub

    Private Sub ButtonSave_MouseHover(sender As Object, e As EventArgs) Handles ButtonSave.MouseHover
        ButtonSave.ForeColor = Color.White
    End Sub

    Private Sub ButtonSave_MouseLeave(sender As Object, e As EventArgs) Handles ButtonSave.MouseLeave
        ButtonSave.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonClearForm_Click(sender As Object, e As EventArgs) Handles ButtonClearForm.Click
        ClearInputUpdateData()
    End Sub

    Private Sub ButtonClearForm_MouseHover(sender As Object, e As EventArgs) Handles ButtonClearForm.MouseHover
        ButtonClearForm.ForeColor = Color.White
    End Sub

    Private Sub ButtonClearForm_MouseLeave(sender As Object, e As EventArgs) Handles ButtonClearForm.MouseLeave
        ButtonClearForm.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonScanID_Click(sender As Object, e As EventArgs) Handles ButtonScanID.Click
        If TimerSerialIn.Enabled = True Then
            PanelReadingTagProcess.Visible = True
            GetID = True
            ButtonScanID.Enabled = False
        Else
            MsgBox("Echec lors de la lecture des données!!!" & vbCr & "Cliquer sur le Menu de Connection ensuite le bouton de connection .", MsgBoxStyle.Critical, "Message d'erreur")
        End If
    End Sub

    Private Sub ButtonScanID_MouseHover(sender As Object, e As EventArgs) Handles ButtonScanID.MouseHover
        ButtonScanID.ForeColor = Color.White
    End Sub

    Private Sub ButtonScanID_MouseLeave(sender As Object, e As EventArgs) Handles ButtonScanID.MouseLeave
        ButtonScanID.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub PictureBoxImageInput_Click(sender As Object, e As EventArgs) Handles PictureBoxImageInput.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPEG (*.jpeg;*.jpg)|*.jpeg;*.jpg"

        If (OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            IMG_FileNameInput = OpenFileDialog1.FileName
            PictureBoxImageInput.ImageLocation = IMG_FileNameInput
        End If
    End Sub

    Private Sub CheckBoxByName_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxByName.CheckedChanged
        If CheckBoxByName.Checked = True Then
            CheckBoxByID.Checked = False
        End If
        If CheckBoxByName.Checked = False Then
            CheckBoxByID.Checked = True
        End If
    End Sub

    Private Sub CheckBoxByID_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxByID.CheckedChanged
        If CheckBoxByID.Checked = True Then
            CheckBoxByName.Checked = False
        End If
        If CheckBoxByID.Checked = False Then
            CheckBoxByName.Checked = True
        End If
    End Sub

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If CheckBoxByID.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                SqlCmdSearchstr = "SELECT nom, id, codePermanent, niveau, ufr FROM  etudiants  ORDER BY nom"
            Else
                SqlCmdSearchstr = "SELECT nom, id, codePermanent, niveau, ufr FROM etudiants  WHERE id LIKE'" & TextBoxSearch.Text & "%'"
            End If
        End If
        If CheckBoxByName.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                SqlCmdSearchstr = "SELECT nom, id, codePermanent, niveau, ufr FROM  etudiants  ORDER BY nom"
            Else
                SqlCmdSearchstr = "SELECT nom, id, codePermanent, niveau, ufr FROM  etudiants  WHERE nom Like'" & TextBoxSearch.Text & "%'"
            End If
        End If

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Echec lors de la connection!!!" & vbCrLf & "Vérifier l'état de votre serveur !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLDA = New MySqlDataAdapter(SqlCmdSearchstr, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                DataGridView1.DataSource = Nothing
                DataGridView1.DataSource = DT
                DataGridView1.DefaultCellStyle.ForeColor = Color.Black
                DataGridView1.ClearSelection()
            Else
                DataGridView1.DataSource = DT
            End If
        Catch ex As Exception
            MsgBox("Echec lors de la recherche" & vbCr & ex.Message, MsgBoxStyle.Critical, "Message d'erreurs")
            Connection.Close()
        End Try
        Connection.Close()
    End Sub

    Private Sub DataGridView1_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        Try
            If AllCellsSelected(DataGridView1) = False Then
                If e.Button = MouseButtons.Left Then
                    DataGridView1.CurrentCell = DataGridView1(e.ColumnIndex, e.RowIndex)
                    Dim i As Integer
                    With DataGridView1
                        If e.RowIndex >= 0 Then
                            i = .CurrentRow.Index
                            LoadImagesStr = True
                            IDRam = .Rows(i).Cells("id").Value.ToString
                            ShowData()
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Return
        End Try
    End Sub

    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        AllCellsSelected = (DataGridView1.SelectedCells.Count = (DataGridView1.RowCount * DataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function

    Private Sub TimerTimeDate_Tick(sender As Object, e As EventArgs) Handles TimerTimeDate.Tick
        LabelDateTime.Text = "Heure " & DateTime.Now.ToString("HH:mm:ss") & "  Date " & DateTime.Now.ToString("dd MMM, yyyy")
    End Sub

    Private Sub SupprimerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupprimerToolStripMenuItem.Click
        If DataGridView1.RowCount = 0 Then
            MsgBox("Impossible de supprimer , la table est vide ! ", MsgBoxStyle.Critical, "Message d'erreur")
            Return
        End If

        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Impossible! sélectionner les données qui doivent etre supprimées ", MsgBoxStyle.Critical, "Message d'erreur")
            Return
        End If

        If MsgBox(" Voulez-vous vraiment supprimer cette enregistrement ?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then Return

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Echec de la connection !!!" & vbCrLf & "Vérifier l'état du serveur !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If AllCellsSelected(DataGridView1) = True Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "DELETE FROM etudiants"
                MySQLCMD.Connection = Connection
                MySQLCMD.ExecuteNonQuery()
            End If

            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                If row.Selected = True Then
                    MySQLCMD.CommandType = CommandType.Text
                    MySQLCMD.CommandText = "DELETE FROM etudiants WHERE id='" & row.DataBoundItem(1).ToString & "'"
                    MySQLCMD.Connection = Connection
                    MySQLCMD.ExecuteNonQuery()
                End If
            Next
        Catch ex As Exception
            MsgBox("Echec lors de la tentative de suppression" & vbCr & ex.Message, MsgBoxStyle.Critical, " Message d'erreur")
            Connection.Close()
        End Try
        PictureBoxImagePreview.Image = Nothing
        Connection.Close()
        ShowData()
    End Sub

    Private Sub ToutSélectionnerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToutSélectionnerToolStripMenuItem.Click
        DataGridView1.SelectAll()
    End Sub

    Private Sub AnnulerSélectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnnulerSélectionToolStripMenuItem.Click
        DataGridView1.ClearSelection()
        PictureBoxImagePreview.Image = Nothing
    End Sub

    Private Sub RéiniatialiserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RéiniatialiserToolStripMenuItem.Click
        ShowData()
    End Sub

    Private Sub TimerSerialIn_Tick(sender As Object, e As EventArgs) Handles TimerSerialIn.Tick
        Try
            StrSerialIn = SerialPort1.ReadExisting
            LabelConnectionStatus.Text = "Statut de la connection: Connecté"
            If StrSerialIn <> "" Then
                If GetID = True Then
                    LabelGetID.Text = StrSerialIn
                    GetID = False
                    If LabelGetID.Text <> "________" Then
                        PanelReadingTagProcess.Visible = False
                        IDCheck()
                    End If
                End If
                If ViewUserData = True Then
                    ViewData()
                End If
            End If
        Catch ex As Exception
            TimerSerialIn.Stop()
            SerialPort1.Close()
            LabelConnectionStatus.Text = "Statut de la connection : Déconnecté"
            PictureBoxStatusConnect.Image = My.Resources.Disconnect
            MsgBox("Echec de connection !!!" & vbCr & "l'Arduino n'est pas détecté.", MsgBoxStyle.Critical, "Message d'erreurs")
            ButtonConnect_Click(sender, e)
            Return
        End Try

        If PictureBoxStatusConnect.Visible = True Then
            PictureBoxStatusConnect.Visible = False
        ElseIf PictureBoxStatusConnect.Visible = False Then
            PictureBoxStatusConnect.Visible = True
        End If
    End Sub

    Private Sub IDCheck()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Echec lors de la Connection !!!" & vbCrLf & "Vérifier le serveur !!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLCMD.CommandType = CommandType.Text
            MySQLCMD.CommandText = "SELECT * FROM  etudiants WHERE id LIKE '" & LabelGetID.Text & "'"
            MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                If MsgBox("ID enregistré !" & vbCr & "Voudriez-vous modififer ces données?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then
                    DT = Nothing
                    Connection.Close()
                    ButtonScanID.Enabled = True
                    GetID = False
                    LabelGetID.Text = "________"
                    Return
                Else
                    Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                    Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                    PictureBoxImageInput.Image = Image.FromStream(lmgStr)
                    PictureBoxImageInput.SizeMode = PictureBoxSizeMode.Zoom

                    TextBoxNom.Text = DT.Rows(0).Item("nom")
                    TextBoxPrenom.Text = DT.Rows(0).Item("codePermanent")
                    TextBoxCodePermanent.Text = DT.Rows(0).Item("niveau")
                    TextBoxSolde.Text = DT.Rows(0).Item("ufr")
                    StatusInput = "Update"
                End If
            End If
        Catch ex As Exception
            MsgBox("Erreur de chargement de la base de données !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()

        ButtonScanID.Enabled = True
        GetID = False
    End Sub

    Private Sub ViewData()
        LabelID.Text = "ID : " & StrSerialIn
        If LabelID.Text = "ID : ________" Then
            ViewData()
        Else
            ShowDataUser()
        End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        GroupBoxImage.Location = New Point((PanelUserData.Width / 2) - (GroupBoxImage.Width / 2), GroupBoxImage.Top)
        PanelReadingTagProcess.Location = New Point((PanelRegistrationandEditUserData.Width / 2) - (PanelReadingTagProcess.Width / 2), 106)
    End Sub

    Private Sub ButtonCloseReadingTag_Click(sender As Object, e As EventArgs) Handles ButtonCloseReadingTag.Click
        PanelReadingTagProcess.Visible = False
        ButtonScanID.Enabled = True
    End Sub


End Class
