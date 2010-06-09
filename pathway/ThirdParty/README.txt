
Public Interface IPrince
    Sub SetEncryptInfo(ByVal keyBits As Integer, _
                       ByVal userPassword As String, _
                       ByVal ownerPassword As String, _
                       ByVal disallowPrint As Boolean, _
                       ByVal disallowModify As Boolean, _
                       ByVal disallowCopy As Boolean, _
                       ByVal disallowAnnotate As Boolean)
    Sub AddStyleSheet(ByVal cssPath As String)
    Sub ClearStyleSheets()
    Sub SetHTML(ByVal html As Boolean)
    Sub SetHttpUser(ByVal user As String)
    Sub SetHttpPassword(ByVal password As String)
    Sub SetHttpProxy(ByVal proxy As String)
    Sub SetLog(ByVal logFile As String)
    Sub SetBaseURL(ByVal baseurl As String)
    Sub SetXInclude(ByVal xinclude As Boolean)
    Sub SetEmbedFonts(ByVal embed As Boolean)
    Sub SetCompress(ByVal compress As Boolean)
    Sub SetEncrypt(ByVal encrypt As Boolean)
    Function Convert(ByVal xmlPath As String) As Boolean
    Function Convert(ByVal xmlPath As String, ByVal pdfPath As String) As Boolean
    Function Convert(ByVal xmlPath As String, ByVal pdfOutput As Stream) As Boolean
    Function Convert(ByVal xmlInput As Stream, ByVal pdfPath As String) As Boolean
    Function Convert(ByVal xmlInput As Stream, ByVal pdfOutput As Stream) As Boolean
    Function ConvertString(ByVal xmlInput As String, ByVal pdfOutput As Stream) As Boolean

End Interface

Public Class Prince
    Implements IPrince
    
    Public Sub New()
    Public Sub New(ByVal princePath As String)

End Class

