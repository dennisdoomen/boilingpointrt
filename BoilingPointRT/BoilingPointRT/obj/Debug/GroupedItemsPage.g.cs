﻿

#pragma checksum "C:\Workspaces\github\boilingpointrt\BoilingPointRT\BoilingPointRT\GroupedItemsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7356524FC9F6BF1D404C33EE355849FD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoilingPointRT
{
    partial class GroupedItemsPage : global::BoilingPointRT.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 51 "..\..\GroupedItemsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 98 "..\..\GroupedItemsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 127 "..\..\GroupedItemsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 107 "..\..\GroupedItemsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Header_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 65 "..\..\GroupedItemsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Header_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

