﻿using System.Collections.Generic;

namespace KarveCommonInterfaces
{
    /// <summary>
    ///  This interface has been used to handle the magnifier and popups based on a list 
    ///  of items.
    ///  It is called interaction request.
    /// </summary>
    public interface IInteractionRequestController
    {
        /// <summary>
        ///  This will show an assist based on the view
        /// </summary>
        /// <typeparam name="T">Data transfer object to be viewed</typeparam>
        /// <param name="title">Title of the window</param>
        /// <param name="dataObjects">Data transfer objects to be shown</param>
        /// <param name="assistProperties">Properties to be used to be filtered</param>
        void ShowAssistView<T>(string title, IEnumerable<T> dataObjects, string assistProperties);
        /// <summary>
        ///  SelectedItem to be shown.
        /// </summary>
        object SelectedItem { get; set; }
        /// <summary>
        ///  SelectionState to be used.
        /// </summary>
        SelectionState SelectionState { get; set; }
    }
}
