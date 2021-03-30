using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterApi.Models
{
	public class QuestViewModel
	{
		public string Id { get; set; }
		public string Description { get; set; }
		[Required]
		public string Status { get; set; }
		public string Title { get; set; }
	}
}