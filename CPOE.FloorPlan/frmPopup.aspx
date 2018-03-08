<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPopup.aspx.cs" Inherits="frmPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .center {
            margin: auto;
            width: 100%;
            /*border: 3px solid #A4C8F0;*/
            padding: 15px;
            text-align:center;
            margin-top:3%;
        }
        .centerbtn {
            margin: auto;
            width: 60%;
            padding: 10px;
            text-align:center;
        }
    </style>
    <link rel="Stylesheet" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1 class="centerbtn">COMFIRM ORDER</h1>
    <div class="center" runat="server" id="divDetail">
        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="table table-bordered table-responsive">
            <Columns>
                <asp:BoundField HeaderText="ARCIM Desc" DataField="ARCIM_Desc" ItemStyle-Width="25%" />
                <asp:BoundField HeaderText="Qty" DataField="OEORI_PhQtyOrd" />
                <asp:BoundField HeaderText="CTUOM Code" DataField="CTUOM_Code" />
                <asp:BoundField HeaderText="PHCFR Desc" DataField="PHCFR_Desc1" />
                <asp:BoundField HeaderText="CTPCP Desc" DataField="CTPCP_Desc" ItemStyle-Width="25%"/>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderID" runat="server" Text='<%# Eval("re_cno") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="centerbtn">
    <asp:Button ID="btnConfirm" CssClass="btn btn-success" runat="server" 
            Text="Confirm" onclick="btnConfirm_Click" />&nbsp;&nbsp;
    <asp:Button ID="btnClose" CssClass="btn" runat="server" Text="Close" OnClientClick="javascript:window.close();" />
    </div>
    
    </form>
</body>
</html>
