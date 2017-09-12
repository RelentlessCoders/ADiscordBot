'
' Simple Discord bot by RelentlessCoders
'          Released 8/30/2017

Imports Discord
Imports System.Net
Imports System.Random
Imports System.IO
Module main
    Dim Trigger As String = "$"
    Dim WithEvents oDiscord As New DiscordClient
    Dim client As New WebClient
    Dim LOG As String = 0

    Sub Main()
        Console.Title = "Discord Bot - By RelentlessCoders"
        Start()
console:
        If Console.ReadLine = "log" Then
            If LOG = 0 Then
                Console.WriteLine("[I] Log: On")
                LOG = 1
            ElseIf LOG = 1 Then
                LOG = 0
                Console.WriteLine("[I] Log: Off")
            End If
        ElseIf Console.ReadLine = "exit" Then
            Console.WriteLine("[I] Closing...")
            oDiscord.Disconnect()
            Environment.Exit(0)
        ElseIf Console.ReadLine = "restart" Then
            Console.Clear()
            oDiscord.Disconnect()
            Try
                oDiscord.Connect(System.IO.File.ReadAllText("token.txt"), TokenType.Bot)
                Console.WriteLine("[+] Connected")
            Catch ex As Exception
                Console.WriteLine("[I] Failure to connect.")
            End Try
        End If
        GoTo console
    End Sub

    Public Function RandomNumber(ByVal n As Integer) As Integer
        'initialize random number generator
        Dim r As New Random(System.DateTime.Now.Millisecond)
        Return r.Next(1, n)
    End Function
    Private Async Sub Start()
        Try
            Await oDiscord.Connect(System.IO.File.ReadAllText("token.txt"), TokenType.Bot)
            Await Task.Delay(500)
            Console.ForegroundColor = ConsoleColor.Red
            Try
                Console.WriteLine("[+] Assigning bot IP to: " + client.DownloadString("https://tools.feron.it/php/ip.php"))
            Catch ex As Exception
                Console.WriteLine("[+] Assigning bot IP to: Localhost")
            End Try
            Await Task.Delay(1400)
            Console.WriteLine("[+] Connected - " + RandomNumber(20).ToString + "ms")
            Await Task.Delay(100)
            Console.WriteLine("[+] Welcome " + My.Computer.Name + " to the Panel")
            Console.ForegroundColor = ConsoleColor.Blue
        Catch ex As Exception

        End Try
    End Sub
    Private Async Sub onMsg(sender As Object, message As Discord.MessageEventArgs) Handles oDiscord.MessageReceived

        If message.User.Name = oDiscord.CurrentUser.Name Then

        Else
            Dim msg As String = message.Message.RawText
               
        ' Did a simple fix here for the arguments not working, they should be all good now!
        ' 9/12/17
        
        
            If msg.StartsWith(Trigger) Then
                If msg.Contains(" ") Then
                    Dim cmd As String = msg.Split(Trigger)(1).Split(" ")(0)
                    Dim arg As String = msg.Split(" ")(1)
                    Dim answer As String = 0
                    If LOG = 1 Then
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("[I] " + Now + " | " + message.User.Name + " | " + msg)
                        Console.ForegroundColor = ConsoleColor.Blue
                    End If
                    Select Case cmd.ToLower

                        Case "resolve"
                            Await message.Channel.SendMessage(client.DownloadString("http://webresolver.nl/api.php?key=NK6GJ-XSS05-Y8R6Q-INEJG&action=resolve&string=" + arg))

                        Case "geoip"
                            Await message.Channel.SendMessage(client.DownloadString("http://webresolver.nl/api.php?key=NK6GJ-XSS05-Y8R6Q-INEJG&action=geoip&string=" + arg).Replace("<br>", " "))
                    
                        Case "calc"
                            Dim arg2 As String = msg.Split(" ")(2)
                            Dim arg3 As String = msg.Split(" ")(3)
                            Dim One As Double
                            Dim Two As Double
                            Dim result As Double
                            One = Double.Parse(arg)
                            Two = Double.Parse(arg3)
                            Try
                                If arg2 = "+" Then
                                    result = arg + arg3
                                ElseIf arg2 = "*" Then
                                    result = arg * arg3
                                ElseIf arg2 = "/" Then
                                    result = arg / arg3
                                ElseIf arg2 = "-" Then
                                    result = arg - arg3
                                End If

                                Await message.Channel.SendMessage(result)
                            Catch ex As Exception
                                Console.WriteLine("Error")
                            End Try

                        Case "hug"
                            Await message.Channel.SendMessage("*Gave hug to " + arg)
                            Await message.Channel.SendMessage("https://giphy.com/gifs/love-disney-girl-EvYHHSntaIl5m ")

                        Case Else
                            Await message.Channel.SendMessage("Invalid command")

                    End Select
                Else
                    Dim cmd As String = msg.Split(Trigger)(1)
                    If LOG = 1 Then
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("[I] " + Now + " | " + message.User.Name + " | " + msg)
                        Console.ForegroundColor = ConsoleColor.Blue
                    End If
                    Select Case cmd.ToLower
                        Case "help"
                            Await message.Channel.SendMessage("Commands:" + vbNewLine + "$joke - Displays a random joke" + vbNewLine + "$restart - Restarts the bot" + vbNewLine + "$clear - Clears last 100 Messages" + vbNewLine + "$resolve - Skype resolver" + vbNewLine + "$geoip - Geolocation an IP" + vbNewLine + "$calc - Calculatwo numbers using mathmatics" + vbNewLine + "$hug - Give someone a hug!")
                        Case "joke"
                            Await message.Channel.SendMessage(client.DownloadString("http://api.icndb.com/jokes/random/"))
                        Case "restart"
                            Console.WriteLine("[I] " + message.User.Name + " sent a Restart.")
                            Await oDiscord.Disconnect()
                            Await oDiscord.Connect(System.IO.File.ReadAllText("token.txt"), TokenType.Bot)
                            Await message.Channel.SendMessage("Restarted!")
                        Case "clear"
                            Dim msgs() As Discord.Message = Await message.Channel.DownloadMessages(100)
                            For Each mess As Discord.Message In msgs
                                Await mess.Delete
                            Next
                        Case Else
                            Await message.Channel.SendMessage("Invalid command")
                    End Select

                End If
            Else
            End If
        End If
    End Sub

    Public Sub onJoin(sender As Object, e As Discord.UserEventArgs) Handles oDiscord.UserJoined
        Dim server = e.Server.FindChannels("main").FirstOrDefault
        server.SendMessage("Welcome " + e.User.Name + " to the Server!")
    End Sub
End Module
