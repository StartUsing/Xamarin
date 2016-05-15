namespace XamarinTest.App
{
    public interface IImageService
    {
        byte[] ResizeImage(byte[] bytes, float maxWidth, float maxHeight);
    }
}