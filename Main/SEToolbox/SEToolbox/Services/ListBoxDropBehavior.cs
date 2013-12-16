﻿// Originally sourced from:
// http://www.codeproject.com/Articles/420545/WPF-Drag-and-Drop-MVVM-using-Behavior
// http://www.dotnetlead.com/wpf-drag-and-drop/application

// Modified to work with MultiSelect, and passing of bound data, and numerous other fixes.

namespace SEToolbox.Services
{
    using SEToolbox.Support;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Interactivity;

    /// <summary>
    /// For enabling Drop on ItemsControl
    /// </summary>
    public class ListBoxDropBehavior : Behavior<ItemsControl>
    {
        #region fields

        private Type dataType; // the type of the data that can be dropped into this control/
        private ListBoxAdornerManager insertAdornerManager;

        #endregion

        #region Properties

        public static readonly DependencyProperty AllowDropToSourceProperty = DependencyProperty.Register("AllowDropToSource", typeof(bool), typeof(ListBoxDropBehavior), new PropertyMetadata(true));
        public bool AllowDropToSource
        {
            get { return (bool)GetValue(AllowDropToSourceProperty); }
            set { SetValue(AllowDropToSourceProperty, value); }
        }

        public static readonly DependencyProperty ShowDropIndicatorProperty = DependencyProperty.Register("ShowDropIndicator", typeof(bool), typeof(ListBoxDropBehavior), new PropertyMetadata(true));
        public bool ShowDropIndicator
        {
            get { return (bool)GetValue(ShowDropIndicatorProperty); }
            set { SetValue(ShowDropIndicatorProperty, value); }
        }

        public static readonly DependencyProperty DropTypeProperty = DependencyProperty.Register("DropType", typeof(Type), typeof(ListBoxDropBehavior));
        /// <summary>
        /// Specify the base Type of the data expected, when the ListBoxItemDragBehavior has set the DragSourceBinding property.
        /// </summary>
        public Type DropType
        {
            get { return (Type)GetValue(DropTypeProperty); }
            set { SetValue(DropTypeProperty, value); }
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Uid = Guid.NewGuid().ToString();
            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.DragEnter += new DragEventHandler(AssociatedObject_DragEnter);
            this.AssociatedObject.DragOver += new DragEventHandler(AssociatedObject_DragOver);
            this.AssociatedObject.DragLeave += new DragEventHandler(AssociatedObject_DragLeave);
            this.AssociatedObject.Drop += new DragEventHandler(AssociatedObject_Drop);
        }

        #region events

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // if the data type can be dropped.
            if (this.dataType != null)
            {
                if (e.Data.GetDataPresent(dataType))
                {
                    // first find the UIElement that it was dropped over, then we determine if it's 
                    // dropped above or under the UIElement, then insert at the correct index.
                    ItemsControl dropContainer = sender as ItemsControl;
                    // get the UIElement that was dropped over.
                    UIElement droppedOverItem = VisualTreeEnumeration.GetUIElement(dropContainer, e.GetPosition(dropContainer));
                    int dropIndex = -1; // the location where the item will be dropped.
                    if (droppedOverItem != null)
                    {
                        dropIndex = dropContainer.ItemContainerGenerator.IndexFromContainer(droppedOverItem) + 1;
                        // find if it was dropped above or below the index item so that we can insert 
                        // the item in the correct place.
                        if (VisualTreeEnumeration.IsPositionAboveElement(droppedOverItem, e.GetPosition(droppedOverItem))) //if above
                        {
                            dropIndex = dropIndex - 1; //we insert at the index above it
                        }
                    }

                    // remove the data from each source.
                    foreach (var item in (IList)e.Data.GetData(dataType))
                    {
                        IDragable source = item as IDragable;
                        if (source != null)
                            source.Remove(item);
                    }

                    // drop the data into destination.
                    IDropable target = this.AssociatedObject.DataContext as IDropable;
                    target.Drop(e.Data.GetData(dataType), dropIndex);
                }
            }

            if (this.insertAdornerManager != null)
            {
                this.insertAdornerManager.Clear();
            }

            e.Handled = true;
            return;
        }

        void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (this.insertAdornerManager != null)
            {
                this.insertAdornerManager.Clear();
            }

            e.Handled = true;
        }

        void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if (this.dataType != null)
            {
                if (e.Data.GetDataPresent(dataType))
                {
                    if (!this.AllowDropToSource && (string)e.Data.GetData(typeof(string)) == ((FrameworkElement)sender).Uid)
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        return;
                    }

                    this.SetDragDropEffects(e);
                    if (this.insertAdornerManager != null && this.ShowDropIndicator)
                    {
                        ItemsControl dropContainer = sender as ItemsControl;
                        UIElement droppedOverItem = VisualTreeEnumeration.GetUIElement(dropContainer, e.GetPosition(dropContainer));
                        if (droppedOverItem != null)
                        {
                            bool isAboveElement = VisualTreeEnumeration.IsPositionAboveElement(droppedOverItem, e.GetPosition(droppedOverItem));
                            this.insertAdornerManager.UpdateDropIndicator(droppedOverItem, isAboveElement);
                        }
                        else
                        {
                            droppedOverItem = (UIElement)dropContainer.ItemContainerGenerator.ContainerFromIndex(dropContainer.Items.Count - 1);
                            this.insertAdornerManager.UpdateDropIndicator(droppedOverItem, false);
                        }
                    }
                }
            }

            e.Handled = true;
        }

        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (this.dataType == null)
            {
                // if the DataContext implements IDropable, record the data type that can be dropped.
                if (this.AssociatedObject.DataContext != null)
                {
                    if (this.AssociatedObject.DataContext as IDropable != null)
                    {
                        if (this.DropType != null)
                            this.dataType = typeof(List<>).MakeGenericType(new[] { this.DropType });
                        else
                            this.dataType = typeof(List<>).MakeGenericType(new[] { ((IDropable)this.AssociatedObject.DataContext).DataType });
                    }
                }
            }

            // initialize adorner manager with the adorner layer of the itemsControl.
            if (this.insertAdornerManager == null)
                this.insertAdornerManager = new ListBoxAdornerManager(AdornerLayer.GetAdornerLayer(sender as ItemsControl));

            e.Handled = true;
        }

        #endregion

        #region helpers

        /// <summary>
        /// Provides feedback on if the data can be dropped.
        /// </summary>
        /// <param name="e"></param>
        private void SetDragDropEffects(DragEventArgs e)
        {
            // if the data type can be dropped.
            if (e.Data.GetDataPresent(dataType))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                // default to None.
                e.Effects = DragDropEffects.None;
            }
        }

        #endregion
    }
}