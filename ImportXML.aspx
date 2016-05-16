<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportXML.aspx.cs" Inherits="TestProj_ImportXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>Import / Export database data from/to XML.</h3>
    <div>
        <table>
            <tr>
                <td>Select File : </td>
                <td>
                    <asp:FileUpload ID="fuFilePath" runat="server" />
                    </td>
                <td>
                    <asp:Button ID="cmdShow" runat="server" Text="Import Data" OnClick="cmdShow_Click" />
                </td>
            </tr>
        </table>
        <div>
            <br />
            <asp:Label ID="lblMessage" runat="server"  Font-Bold="true" />
            <br />
            <asp:GridView ID="gvUpload2" runat="server" AutoGenerateColumns="true">
                <EmptyDataTemplate>
                    <div style="padding:10px">
                        Data not found!
                    </div>
                </EmptyDataTemplate>
                <%--<Columns>
                    <asp:BoundField HeaderText="Employee ID" DataField="EmployeeID" />
                    <asp:BoundField HeaderText="Company Name" DataField="CompanyName" />
                    <asp:BoundField HeaderText="Contact Name" DataField="ContactName" />
                    <asp:BoundField HeaderText="Contact Title" DataField="ContactTitle" />
                    <asp:BoundField HeaderText="Address" DataField="EmployeeAddress" />
                    <asp:BoundField HeaderText="Postal Code" DataField="PostalCode" />
                </Columns>--%>
                <HeaderStyle BackColor="Gray" Font-Bold="True" Height="30px" ForeColor="White" />
            </asp:GridView>
            <br />
            <%--<asp:Button ID="btnExport" runat="server" Text="Export Data" OnClick="btnExport_Click" />--%>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
