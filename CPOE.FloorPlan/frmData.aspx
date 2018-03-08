<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmData.aspx.cs" Inherits="frmData" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Floor Plan</title>
    <link rel="shortcut icon" type="image/x-icon" href="imgs/favicon.ico" />
    <link rel="icon" href="imgs/favicon.png" type="image/png" />

    <script type="text/javascript">
        setTimeout(
        function () {
            window.location.reload();
           
        }, 60000);

        function fnCheckAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
        function removetagsHtml() {
            
        }


    </script>

    <style type="text/css">
        .highlight {
            background-color: darkorange;
        }

        .img {
            width: 50px;
            height: 40px;
        }

        .mydatagrid {
            width: 100%;
            border: solid 2px white;
        }

        .header {
            /*background-color: #63a3fa; */
            background-color: #75B4FF;
            font-family: Tahoma;
            color: White;
            height: 25px;
            text-align: center;
            font-size: 14px;
            font-weight: bold;
        }

        .rows {
            background-color: #fff;
            font-family: Tahoma;
            font-size: 14px;
            height: 25px;
            /*color: #000;*/
            color: #62A9FF;
            min-height: 25px;
            text-align: left;
        }

            .rows:hover {
                background-color: #d9effa;
                color: #77cdfa;
            }

        .mydatagrid a /** FOR THE PAGING ICONS  **/ {
            /*background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: #fff;
            text-decoration: none;
            font-weight: bold;*/
        }

            .mydatagrid a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                /*background-color: #000;
                color: #fff;*/
            }

        .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            background-color: #fff;
            color: #000;
            padding: 5px 5px 5px 5px;
        }

        .pager {
            background-color: #5badff;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid td {
            padding: 5px;
        }

        .mydatagrid th {
            padding: 5px;
        }

        .button1 {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            padding: 10px 24px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
            border-radius: 4px;
        }


        div.tooltips {
            position: relative;
            display: inline;
        }

            div.tooltips span {
                position: absolute;
                width: 500px;
                color: greenyellow;
                background: #121212;
                line-height: 20px;
                text-align: left;
                visibility: hidden;
                border-radius: 20px;
                white-space: pre;
                padding: 10px;
            }

                div.tooltips span:after {
                    content: '';
                    position: absolute;
                    top: 100%;
                    left: 50%;
                    margin-left: -8px;
                    width: 0;
                    height: 0;
                    border-top: 8px solid #121212;
                    border-right: 8px solid transparent;
                    border-left: 8px solid transparent;
                }

        div:hover.tooltips span {
            visibility: visible;
            opacity: 0.8;
            bottom: 30px;
            left: 50%;
            margin-left: -76px;
            z-index: 999;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <img src="imgs/header.png" alt="" style="width: 100%;" />
        </div>
        <div>
            <br />
            <%--           <asp:UpdatePanel runat="server">
               <ContentTemplate>--%>
            <asp:GridView ID="grid" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                OnRowDataBound="grid_RowDataBound" OnPageIndexChanging="grid_PageIndexChanging" PageSize="20"
                OnDataBound="grid_DataBound" AllowPaging="True" OnRowCommand="grid_RowCommand" OnRowCreated="grid_RowCreated">
                <Columns>
                    <asp:TemplateField>

                        <HeaderTemplate>
                            Select<br />
                            <br />
                            <asp:CheckBox ID="CheckAll" runat="server" onclick="fnCheckAll(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckID" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="OEORI_RowId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblOEORI_RowId" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- <asp:TemplateField HeaderText="Order Detail" Visible="true" >

                        <ItemTemplate >
                            <asp:Label ID="ARCIM_Desc" runat="server" Text='<%# Eval("ARCIM_Desc") %>' ></asp:Label>
                            <br />    
                           <asp:Label ID="APIOrder" runat="server" Text='<%# Eval("APIOrder") %>' ></asp:Label>
 
                        </ItemTemplate>
                        <ControlStyle      Font-Names="Tahoma" ForeColor="#62A9FF" CssClass="mydatagrid" Font-Size="14px" Height="15"  />

                    </asp:TemplateField>--%>

                    <%--<asp:BoundField DataField="ARCIM_Desc" HeaderText="Order Detail" HtmlEncode="False" HtmlEncodeFormatString ="False">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Order Detail" ItemStyle-Width="30%">
                        <ItemTemplate>
                            <div class="tooltips" id="showTooltip" runat="server" href="#">
                                <%# Eval("ARCIM_Desc") %>
                                <span style='<%# (Eval("APIOrder").ToString() == "")? "visibility:hidden": "" %>'><%# HttpUtility.HtmlDecode(Eval("APIOrder").ToString().Replace("<br/>", "\r\n")) %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OEORI_SttDat" HeaderText="Order Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OEORI_SttTim" HeaderText="Order Time">
                        <ItemStyle Width="7%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OECPR_Desc" HeaderText="Status">
                        <ItemStyle Width="7%" />
                    </asp:BoundField>
                    <%--                    <asp:BoundField DataField="BED_Code" HeaderText="Bed">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Bed">
                        <ItemTemplate>
                            <%-- <asp:LinkButton ID="lbBedcode" runat="server" Text='<%# Eval("BED_Code") %>'
                                 CommandName="CheckAllRoomSelect" CommandArgument='<%#Bind("BED_Code") %>' ToolTip ='<%# Eval("APIOrder") %>'>
                             </asp:LinkButton>--%>
                            <asp:LinkButton ID="lbBedcode" runat="server" Text='<%# Eval("BED_Code") %>'
                                CommandName="CheckAllRoomSelect" CommandArgument='<%#Bind("BED_Code") %>'>
                            </asp:LinkButton>
                            <asp:Label ID="BED_Code" runat="server" Text='<%# Eval("BED_Code") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblAPIOrder" runat="server" Text='<%# Eval("APIOrder") %>' Visible="false"></asp:Label>
                            <%-- <asp:HiddenField ID="lblAPIOrder" runat="server" Value='<%# Eval("APIOrder") %>' />--%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CTPCP_Desc" HeaderText="Doctor">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="APIOrder" HeaderText="CPOE OrderDetail" Visible ="true" HtmlEncode="False" HtmlEncodeFormatString="False" >
                        <ItemStyle Width="5px" />
                    </asp:BoundField>--%>
                </Columns>

                <FooterStyle BackColor="#75B4FF" />

                <HeaderStyle CssClass="header"></HeaderStyle>

                <PagerStyle CssClass="pager"></PagerStyle>

                <RowStyle CssClass="rows"></RowStyle>
            </asp:GridView>
            <%--               </ContentTemplate>
           </asp:UpdatePanel>--%>

            <hr />

            <asp:Button ID="btCheckOrder" runat="server" Text="Confirm Order" OnClick="btCheckOrder_Click" CssClass="button1" />
            <asp:Label ID="lblText" runat="server"></asp:Label>

        </div>
    </form>

</body>
</html>
