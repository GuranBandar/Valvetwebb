<%@ Page Title="ValvPost" Language="C#" AutoEventWireup="true" MasterPageFile="~/App.Master"
    CodeBehind="ValvPostInfo.aspx.cs" Inherits="Valvetwebb.ValvPostInfo" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <script type="text/javascript">
        function Confirm_Delete() {
            var confirm_value = document.getElementById('confirm_value');

            if (!confirm_value) { //create element only if not found
                confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                confirm_value.id = "confirm_value";

                document.forms[0].appendChild(confirm_value);
            }

            if (confirm("Säkert att du vill ta bort hela posten?")) {
                confirm_value.value = "Ja";
            } else {
                confirm_value.value = "Nej";
            }
        }
    </script>
    <div>
        <table id="TableHeader" class="custom-table" runat="server">
            <tr>
                <td></td>
                <td>
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="form">
            <table id="TableValvpost" style="margin: auto; border: 5px solid darkgray"
                class="custom-table" runat="server">
                <tr>
                    <td style="height: 5px" align="center"></td>
                </tr>
                <tr>
                    <td>
                        <table class="custom-table">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Width="1%" runat="server"></asp:Label>
                                    <asp:Label ID="lblPostnamn" runat="server" CssClass="headerStyle" Text="Postnamn" Enabled="true" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" Width="1%" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtPostnamn" runat="server" CssClass="brodtext" Width="200" Enabled="true" Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px" align="center"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" Height="3px" Width="2%" runat="server"></asp:Label>
                                    <asp:Label ID="lblUsernamn" runat="server" CssClass="rubriktext" Text="Usernamn"></asp:Label>
                                    <asp:Label ID="Label1" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUsernamn" runat="server" CssClass="brodtext" Height="21" Width="200" Enabled="true"></asp:TextBox>
                                    <asp:Label ID="Label7" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" Height="3px" Width="2%" runat="server"></asp:Label>
                                    <asp:Label ID="lblLosenord" runat="server" CssClass="rubriktext" Text="Lösenord "></asp:Label>
                                    <asp:Label ID="Label19" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtLosenord" runat="server" Height="21" Width="200" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowPass" runat="server" CssClass="brodtext" Text="Visa lösenord" OnCheckedChanged="chkShowPass_Click" AutoPostBack="true" />
                                    <asp:Label ID="Label20" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label21" Height="3px" Width="2%" runat="server"></asp:Label>
                                    <asp:Label ID="lblWebbadress" runat="server" CssClass="rubriktext" Text="Webbadress"></asp:Label>
                                    <asp:Label ID="Label23" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtWebadress" runat="server" CssClass="brodtext" Height="21" Width="200" Enabled="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LkbLanka" runat="server" text="Länka" OnClick="LkbLanka_Click" Enabled="true"></asp:LinkButton>
                                    <asp:Label ID="Label4" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" Height="3px" Width="2%" runat="server"></asp:Label>
                                    <asp:Label ID="lblAnteckningar" runat="server" CssClass="rubriktext" Text="Anteckningar"></asp:Label>
                                    <asp:Label ID="Label25" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtAnteckningar" runat="server" TextMode="MultiLine" Width="200" Height="100"></asp:TextBox>
                                    <asp:Label ID="Label26" Height="3px" Width="2%" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" align="center"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="knappSpara" Text="Spara" OnClick="knappSpara_Click" Style="cursor: hand" runat="server" CssClass="ButtonClass"></asp:Button>
                        <asp:Button ID="knappNy" Text="Ny post" OnClick="knappNy_Click" Style="cursor: hand" runat="server" CssClass="ButtonClass"></asp:Button>
                        <asp:Button ID="knappTaBort" Text="Ta bort" OnClick="knappTaBort_Click" Style="cursor: hand" runat="server" CssClass="ButtonClass"
                            OnClientClick="Confirm_Delete()"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" align="center"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="knappTillbaka" Text="Tillbaka" OnClick="knappTillbaka_Click" Style="cursor: hand" runat="server" CssClass="ButtonClass"></asp:Button>
                        <asp:Button ID="knappLogout" Text="Logga ut" OnClick="knappLogout_Click" Style="cursor: hand" runat="server" CssClass="ButtonClass"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" Height="3px" Width="2%" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

