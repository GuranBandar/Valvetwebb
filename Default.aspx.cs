﻿using System;
using System.Web.UI;

namespace Valvet
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }
    }
}