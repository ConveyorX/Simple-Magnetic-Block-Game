using UnityEditor;
using UnityEngine;

public class HelpMenuItem : MonoBehaviour
{
    [MenuItem("SansDev/Help/Documentation (online)")]
    static void OpenDocumantation()
    {
        Application.OpenURL($"https://docs.google.com/document/d/10XvDqK_smJN2KJWz0IUPwj-Gcq-6goB_IO2cO55YkAU");
    }

    [MenuItem("SansDev/Help/More Games")]
    static void OpenPortofolio()
    {
        Application.OpenURL($"https://codecanyon.net/user/sansdevs/portfolio");
    }
}
