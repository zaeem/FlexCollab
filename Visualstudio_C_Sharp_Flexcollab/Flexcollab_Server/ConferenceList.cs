using System;
using System.Collections;


namespace WebMeeting.Server
{
	/// <summary>
	/// A special data structure used to store list of conferences. 
	/// Conferences can be searched efficiently based on their id.
	/// </summary>
	public class ConferenceList
	{
        /// <summary>
        /// A hashtable used to store Conferences
        /// </summary>
        public Hashtable        ConfList;		

        /// <summary>
        /// Public constructor used to instantiate the ConferenceList
        /// </summary>
		public ConferenceList()
		{
            //
            // Allocate the hashtable. Make it thread safe for write operations.
            //
            ConfList = Hashtable.Synchronized(new Hashtable());
			
		}

        /// <summary>
        /// Use this method to add a new conference to this ConferenceList. If
        /// a meeting with this Id already exists, ArgumentException is thrown.
        /// </summary>
        /// <param name="confId"> Id of the Conference to be added </param>
        /// <param name="conference"> Reference to Conference object associated with Id </param>
        public void Add(String confId, ConferenceRoom conference)
        {
            ConfList.Add(confId, conference);
        }

        /// <summary>
        /// Search for a meeting in the Conf List
        /// </summary>
        /// <param name="confId"> The Id for the meeting </param>
        /// <returns> Reference to the conference object associated with
        /// this meeting. If meeting is not found, returns null </returns>
        public ConferenceRoom FindConference(String confId)
        {
            return ((ConferenceRoom) ConfList[confId]);
        }

        /// <summary>
        /// Checks if the specified Conference Id corresponds to any 
        /// conference present in this list.
        /// </summary>
        /// <param name="confId"> The conference Id to search on </param>
        /// <returns> True if conference exists, else false </returns>
        public bool Exists(String confId)
        {
            return ConfList.ContainsKey(confId);
        }
	}
}
