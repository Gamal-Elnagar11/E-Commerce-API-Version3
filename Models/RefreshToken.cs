using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_API.Models
{
    public class RefreshToken
    {
        
            public int Id { get; set; }

            // التوكن نفسه (سلسلة حروف وأرقام عشوائية طويلة)
            public string Token { get; set; }

            // تاريخ الانتهاء (مثلاً 7 أيام من وقت الإنشاء)
            public DateTime Expires { get; set; }

            // هل تم استخدامه بالفعل؟ (عشان نمنع إعادة استخدام نفس التوكن)
            public bool IsUsed { get; set; }

            // هل تم إلغاؤه (في حالة الـ Logout)
            public bool IsRevoked { get; set; }

            // الربط بالأيدي (ForeignKey)
            public string UserId { get; set; }

            [ForeignKey(nameof(UserId))]
            public User User { get; set; }
        
    }
}
