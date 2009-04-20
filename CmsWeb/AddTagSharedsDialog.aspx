﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTagSharedsDialog.aspx.cs"
    Inherits="CMSWeb.AddTagSharedsDialog" StylesheetTheme="Minimal" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="user" TagName="QuickSearchParameters" Src="UserControls/QuickSearchParameters.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Add Contactor Dialog</title>
</head>

<script type="text/javascript">
    function ToggleCallback(e) {
        var result = eval('(' + e + ')');
        $get(result.ControlId).checked = result.HasTag;
    }
</script>

<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="retval" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <user:QuickSearchParameters ID="Parameters" runat="server" />
        <asp:Button ID="AddSelectedUsers" runat="server" OnClick="AddSelectedUsers_Click"
            Text="Add Selected" />
        <div style="overflow: auto; height: 220px;">
            <asp:ListView ID="ListView1" runat="server" DataKeyNames="PeopleId">
                <LayoutTemplate>
                    <table id="table1" runat="server">
                        <tr id="tr2" runat="server">
                            <td runat="server">
                                <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                    <tr runat="server" style="">
                                        <th runat="server">
                                            Select
                                        </th>
                                        <th runat="server">
                                            Name
                                        </th>
                                        <th runat="server">
                                            Address
                                        </th>
                                        <th runat="server">
                                            CityStateZip
                                        </th>
                                        <th runat="server">
                                            Age
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'
                        title='<%# Eval("ToolTip") %>'>
                        <td>
                            <asp:CheckBox ID="ck" runat="server" Visible='<%# (int)Eval("PeopleId")>0 %>' Checked='<%# (bool)Eval("HasTag") %>' onclick='<%# Eval("PeopleId", "PageMethods.ToggleTag({0}, this.id, ToggleCallback); return false;") %>'>
                            </asp:CheckBox>
                        </td>
                        <td>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td>
                            <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                        </td>
                        <td>
                            <asp:Label ID="CityStateZipLabel" runat="server" Text='<%# Eval("CityStateZip") %>' />
                        </td>
                        <td>
                            <asp:Label ID="AgeLabel" runat="server" Text='<%# Eval("Age") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>
                                No data was returned.
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
    </form>
</body>
</html>
