#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("nNV5da8Vbh5oxhQzDEfT6mJZLuX6Ge0DVlFL21GLo6FN0JrXGvNVlPAwRHzGI86RGWSe9awrgSDin2a71GlNcjyzc/TsnkDagdow6OJt6yP5S8jr+cTPwONPgU8+xMjIyMzJyjokcqgVQu0sngvDiK3p6pkqhXYa0NxABNwE58m4RFnks6d3sACRn+Kd9zzecsG0TLylLcP7jhBq6c/stWOFi4sypa4ta3iWOC9JErsE2F/evHv5CKefhkoY4cqtGqBd4CCjnWZLyMbJ+UvIw8tLyMjJVcR8648tEB+LAlKsqWfszGuOqdqGeaHOLSRzhUEWr3YTdFVYOnecdLaRiammCiiekegt4PXxYvaZkghWjC7gPgN3xEzxMeBDpz87iMvKyMnI");
        private static int[] order = new int[] { 11,7,5,12,11,10,6,9,12,13,13,12,12,13,14 };
        private static int key = 201;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
