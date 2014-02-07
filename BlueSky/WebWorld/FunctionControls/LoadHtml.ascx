<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadHtml.ascx.cs" Inherits="WebWorld.FunctionControls.LoadHtml" %>
<script type="text/javascript">
    $(document).ready(function() {
        $.ajax({
            url: "Window.aspx?value=FunctionControls/CodeTest.ascx",
            cache: false,
            success: function(msg) {
                $("#showHtml").html(msg);
            }
        });
    });
</script>
<div id="showHtml"></div>