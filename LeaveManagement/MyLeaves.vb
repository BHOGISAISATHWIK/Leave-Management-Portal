﻿Public Class MyLeaves

    Private Sub leaves_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' Establish a connection to the MySQL database
            Using connection As New MySqlConnection(My.Settings.connectionString)
                connection.Open()

                Dim UserEmail As String = Environment.GetEnvironmentVariable("userEmail")
                ' Define the SQL query to fetch data
                Dim query As String = "SELECT type as Nature, from_date, to_date, reason, status, reply_date " &
                                      "FROM requests " &
                                      "WHERE applicant_email = @UserEmail AND status IN ('approved', 'declined')" &
                                      "ORDER BY reply_date DESC"


                ' Create a MySqlCommand object to execute the query
                Using cmd As New MySqlCommand(query, connection)
                    ' Add parameters to the query to filter by user email
                    cmd.Parameters.AddWithValue("@UserEmail", UserEmail)

                    ' Execute the query and fetch the data into a DataTable
                    Dim dataTable As New DataTable()
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dataTable)
                    End Using


                    If dataTable.Rows.Count = 0 Then
                        Dim nodatalabel As New Label()
                        nodatalabel.Text = "No leaves taken!!"
                        nodatalabel.AutoSize = True
                        nodatalabel.ForeColor = Color.Green
                        nodatalabel.Font = New Font(nodatalabel.Font.FontFamily, 10)
                        nodatalabel.Padding = New Padding(5)
                        nodatalabel.TextAlign = ContentAlignment.MiddleCenter
                        DataGridView1.Visible = True
                        Me.Controls.Add(nodatalabel)
                        nodatalabel.Top = Panel1.Top + 30
                        nodatalabel.Left = DataGridView1.Left
                    Else
                        ' Display the fetched data in the GroupBox
                        DataGridView1.DataSource = dataTable
                        'DisplayRequestsInGroupBox(dataTable)
                        DataGridView1.AllowUserToAddRows = False
                        DataGridView1.RowHeadersVisible = False
                        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                        DataGridView1.ScrollBars = ScrollBars.Vertical
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class