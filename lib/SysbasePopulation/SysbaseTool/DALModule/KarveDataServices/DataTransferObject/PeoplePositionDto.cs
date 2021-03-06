﻿namespace KarveDataServices.DataTransferObject
{
    /// <summary>
    ///  PeoplePositionDto.
    /// </summary>
    public class PeoplePositionDto: BaseDto
    {
        /// <summary>
        ///  Code 
        /// </summary>
        public byte Code { set; get; }
        /// <summary>
        ///  Name.
        /// </summary>
        public string Name { set; get; }
    }
}