﻿#pragma checksum "..\..\BusInfo.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1EF0C9A28DF1C8C68BE6D1358B4F4E7BCF97195DDF95751084431730F2C57AAA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using dotNet5781_03B_6656_9835;


namespace dotNet5781_03B_6656_9835 {
    
    
    /// <summary>
    /// BusInfo
    /// </summary>
    public partial class BusInfo : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\BusInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbInfo;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\BusInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditName;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\BusInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Service;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\BusInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Refuel;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\BusInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbAddName;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/dotNet5781_03B_6656_9835;component/businfo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\BusInfo.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tbInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.EditName = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\BusInfo.xaml"
            this.EditName.Click += new System.Windows.RoutedEventHandler(this.EditName_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Service = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\BusInfo.xaml"
            this.Service.Click += new System.Windows.RoutedEventHandler(this.Service_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Refuel = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\BusInfo.xaml"
            this.Refuel.Click += new System.Windows.RoutedEventHandler(this.Refuel_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tbAddName = ((System.Windows.Controls.TextBox)(target));
            
            #line 27 "..\..\BusInfo.xaml"
            this.tbAddName.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.tbAddName_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
