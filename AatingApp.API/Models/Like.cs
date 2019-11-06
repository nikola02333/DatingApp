using System;

namespace AatingApp.API.Models
{
    public class Like
    {
    public int LikerId { get; set; }
    public int  LikeeId { get; set; }
    public  User Liker {set; get;}

    public  User Likee {set; get;}

    }
}