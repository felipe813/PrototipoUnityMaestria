//The implementation is based on this article:http://rbarraza.com/html5-canvas-pageflip/
//As the rbarraza.com website is not live anymore you can get an archived version from web archive 
//or check an archived version that I uploaded on my website: https://dandarawy.com/html5-canvas-pageflip/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Threading;
public enum FlipMode
{
    RightToLeft,
    LeftToRight
}
[ExecuteInEditMode]
public class Book : MonoBehaviour {
    public Canvas canvas;
    [SerializeField]
    RectTransform BookPanel;

    public Sprite hojaIzquierda;
    public Sprite hojaDerecha;
    public Sprite cinta;
    public Sprite fondoFoto;
    public Sprite cubierta;
    public Sprite cubiertaAtras;
    public GameObject notas;
    public GameObject titulo;

    public Sprite background;
    private Sprite[] bookPages;
    public bool interactable=true;
    public bool enableShadowEffect=true;
    //represent the index of the sprite shown in the right page
    public int currentPage = 0;
    public int TotalPageCount
    {
        get { return bookPages.Length; }
    }
    public Vector3 EndBottomLeft
    {
        get { return ebl; }
    }
    public Vector3 EndBottomRight
    {
        get { return ebr; }
    }
    public float Height
    {
        get
        {
            return BookPanel.rect.height ; 
        }
    }
    public Image ClippingPlane;
    public Image NextPageClip;
    public Image Shadow;
    public Image ShadowLTR;
    public Image Left;
    public Image LeftNext;
    public Image Right;
    public Image RightNext;
    public UnityEvent OnFlip;
    float radius1, radius2;
    //Spine Bottom
    Vector3 sb;
    //Spine Top
    Vector3 st;
    //corner of the page
    Vector3 c;
    //Edge Bottom Right
    Vector3 ebr;
    //Edge Bottom Left
    Vector3 ebl;
    //follow point 
    Vector3 f;
    bool pageDragging = false;
    //current flip mode
    FlipMode mode;


    void Start()
    {

    }
    public void StartBook(List<int> Ids)
    {
        CrearPaginas(Ids);

        if (!canvas) canvas=GetComponentInParent<Canvas>();
        if (!canvas) Debug.LogError("Book should be a child to canvas");

        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        UpdateSprites();
        CalcCurlCriticalPoints();

        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        NextPageClip.rectTransform.sizeDelta = new Vector2(pageWidth, pageHeight + pageHeight * 2);


        ClippingPlane.rectTransform.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);

        //hypotenous (diagonal) page length
        float hyp = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
        float shadowPageHeight = pageWidth / 2 + hyp;

        Shadow.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        Shadow.rectTransform.pivot = new Vector2(1, (pageWidth / 2) / shadowPageHeight);

        ShadowLTR.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        ShadowLTR.rectTransform.pivot = new Vector2(0, (pageWidth / 2) / shadowPageHeight);

        foreach (int id in Ids)
            Recorrido.OcultarImagen(id);
    }

    private void CalcCurlCriticalPoints()
    {      
        sb = new Vector3(0, -BookPanel.rect.height / 2);
        ebr = new Vector3(BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        ebl = new Vector3(-BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        st = new Vector3(0, BookPanel.rect.height / 2);
        radius1 = Vector2.Distance(sb, ebr);
        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        radius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
    }

    public Vector3 transformPoint(Vector3 mouseScreenPos)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 mouseWorldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, canvas.planeDistance));
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseWorldPos);

            return localPos;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 globalEBR = transform.TransformPoint(ebr);
            Vector3 globalEBL = transform.TransformPoint(ebl);
            Vector3 globalSt = transform.TransformPoint(st);
            Plane p = new Plane(globalEBR, globalEBL, globalSt);
            float distance;
            p.Raycast(ray, out distance);
            Vector2 localPos = BookPanel.InverseTransformPoint(ray.GetPoint(distance));
            return localPos;
        }
        else
        {
            //Screen Space Overlay
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseScreenPos);
            return localPos;
        }
    }
    void Update()
    {
        if (pageDragging && interactable)
        {
            UpdateBook();
        }
    }
    public void UpdateBook()
    {
        f = Vector3.Lerp(f, transformPoint(Input.mousePosition), Time.deltaTime * 10);
        if (mode == FlipMode.RightToLeft)
            UpdateBookRTLToPoint(f);
        else
            UpdateBookLTRToPoint(f);
    }
    public void UpdateBookLTRToPoint(Vector3 followLocation)
    {
        mode = FlipMode.LeftToRight;
        f = followLocation;
        ShadowLTR.transform.SetParent(ClippingPlane.transform, true);
        ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
        ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
        Left.transform.SetParent(ClippingPlane.transform, true);

        Right.transform.SetParent(BookPanel.transform, true);
        Right.transform.localEulerAngles = Vector3.zero;
        LeftNext.transform.SetParent(BookPanel.transform, true);

        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebl, out t1);
        //0 < T0_T1_Angle < 180
        clipAngle = (clipAngle + 180) % 180;

        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        //page position and angle
        Left.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        Left.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        LeftNext.transform.SetParent(NextPageClip.transform, true);
        Right.transform.SetParent(ClippingPlane.transform, true);
        Right.transform.SetAsFirstSibling();

        ShadowLTR.rectTransform.SetParent(Left.rectTransform, true);
    }
    public void UpdateBookRTLToPoint(Vector3 followLocation)
    {
        mode = FlipMode.RightToLeft;
        f = followLocation;
        Shadow.transform.SetParent(ClippingPlane.transform, true);
        Shadow.transform.localPosition = Vector3.zero;
        Shadow.transform.localEulerAngles = Vector3.zero;
        Right.transform.SetParent(ClippingPlane.transform, true);

        Left.transform.SetParent(BookPanel.transform, true);
        Left.transform.localEulerAngles = Vector3.zero;
        RightNext.transform.SetParent(BookPanel.transform, true);
        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebr, out t1);
        if (clipAngle > -90) clipAngle += 180;

        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);
        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        //page position and angle
        Right.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        Right.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - (clipAngle + 90));

        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        RightNext.transform.SetParent(NextPageClip.transform, true);
        Left.transform.SetParent(ClippingPlane.transform, true);
        Left.transform.SetAsFirstSibling();

        Shadow.rectTransform.SetParent(Right.rectTransform, true);
    }
    private float CalcClipAngle(Vector3 c,Vector3 bookCorner,out  Vector3 t1)
    {
        Vector3 t0 = (c + bookCorner) / 2;
        float T0_CORNER_dy = bookCorner.y - t0.y;
        float T0_CORNER_dx = bookCorner.x - t0.x;
        float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);
        float T0_T1_Angle = 90 - T0_CORNER_Angle;
        
        float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
        T1_X = normalizeT1X(T1_X, bookCorner, sb);
        t1 = new Vector3(T1_X, sb.y, 0);
        
        //clipping plane angle=T0_T1_Angle
        float T0_T1_dy = t1.y - t0.y;
        float T0_T1_dx = t1.x - t0.x;
        T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;
        return T0_T1_Angle;
    }
    private float normalizeT1X(float t1,Vector3 corner,Vector3 sb)
    {
        if (t1 > sb.x && sb.x > corner.x)
            return sb.x;
        if (t1 < sb.x && sb.x < corner.x)
            return sb.x;
        return t1;
    }
    private Vector3 Calc_C_Position(Vector3 followLocation)
    {
        Vector3 c;
        f = followLocation;
        float F_SB_dy = f.y - sb.y;
        float F_SB_dx = f.x - sb.x;
        float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
        Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_SB_Angle),radius1 * Mathf.Sin(F_SB_Angle), 0) + sb;

        float F_SB_distance = Vector2.Distance(f, sb);
        if (F_SB_distance < radius1)
            c = f;
        else
            c = r1;
        float F_ST_dy = c.y - st.y;
        float F_ST_dx = c.x - st.x;
        float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);
        Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_ST_Angle),
           radius2 * Mathf.Sin(F_ST_Angle), 0) + st;
        float C_ST_distance = Vector2.Distance(c, st);
        if (C_ST_distance > radius2)
            c = r2;
        return c;
    }
    public void DragRightPageToPoint(Vector3 point)
    {
        if (currentPage >= bookPages.Length) return;
        pageDragging = true;
        mode = FlipMode.RightToLeft;
        f = point;


        NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

        Left.gameObject.SetActive(true);
        Left.rectTransform.pivot = new Vector2(0, 0);
        Left.transform.position = RightNext.transform.position;
        Left.transform.eulerAngles = new Vector3(0, 0, 0);
        Left.sprite = (currentPage < bookPages.Length) ? bookPages[currentPage] : background;
        Left.transform.SetAsFirstSibling();
        
        Right.gameObject.SetActive(true);
        Right.transform.position = RightNext.transform.position;
        Right.transform.eulerAngles = new Vector3(0, 0, 0);
        Right.sprite = (currentPage < bookPages.Length - 1) ? bookPages[currentPage + 1] : background;

        RightNext.sprite = (currentPage < bookPages.Length - 2) ? bookPages[currentPage + 2] : background;

        LeftNext.transform.SetAsFirstSibling();
        if (enableShadowEffect) Shadow.gameObject.SetActive(true);
        UpdateBookRTLToPoint(f);
    }
    public void OnMouseDragRightPage()
    {
        if (interactable)
        DragRightPageToPoint(transformPoint(Input.mousePosition));
        
    }
    public void DragLeftPageToPoint(Vector3 point)
    {
        if (currentPage <= 0) return;
        pageDragging = true;
        mode = FlipMode.LeftToRight;
        f = point;

        NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);

        Right.gameObject.SetActive(true);
        Right.transform.position = LeftNext.transform.position;
        Right.sprite = bookPages[currentPage - 1];
        Right.transform.eulerAngles = new Vector3(0, 0, 0);
        Right.transform.SetAsFirstSibling();

        Left.gameObject.SetActive(true);
        Left.rectTransform.pivot = new Vector2(1, 0);
        Left.transform.position = LeftNext.transform.position;
        Left.transform.eulerAngles = new Vector3(0, 0, 0);
        Left.sprite = (currentPage >= 2) ? bookPages[currentPage - 2] : background;

        LeftNext.sprite = (currentPage >= 3) ? bookPages[currentPage - 3] : background;

        RightNext.transform.SetAsFirstSibling();
        if (enableShadowEffect) ShadowLTR.gameObject.SetActive(true);
        UpdateBookLTRToPoint(f);
    }
    public void OnMouseDragLeftPage()
    {
        if (interactable)
        DragLeftPageToPoint(transformPoint(Input.mousePosition));
        
    }
    public void OnMouseRelease()
    {
        if (interactable)
            ReleasePage();
    }
    public void ReleasePage()
    {
        if (pageDragging)
        {
            pageDragging = false;
            float distanceToLeft = Vector2.Distance(c, ebl);
            float distanceToRight = Vector2.Distance(c, ebr);
            if (distanceToRight < distanceToLeft && mode == FlipMode.RightToLeft)
                TweenBack();
            else if (distanceToRight > distanceToLeft && mode == FlipMode.LeftToRight)
                TweenBack();
            else
                TweenForward();
        }
    }
    Coroutine currentCoroutine;
    void UpdateSprites()
    {
        LeftNext.sprite= (currentPage > 0 && currentPage <= bookPages.Length) ? bookPages[currentPage-1] : background;
        RightNext.sprite=(currentPage>=0 &&currentPage<bookPages.Length) ? bookPages[currentPage] : background;
    }
    public void TweenForward()
    {
        if(mode== FlipMode.RightToLeft)
        currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f, () => { Flip(); }));
        else
        currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f, () => { Flip(); }));
    }

    private void SiguienteImagen(ControladorRecorrido Controlador){
            Debug.Log("Siguiente");
            Controlador.SiguienteImagen();
    }

    private void AnteriorImagen(ControladorRecorrido Controlador){
            Debug.Log("Anterior");         
            Controlador.AnteriorImagen();
    }

    void Flip()
    {      
        GameObject Controlador = GameObject.Find("Controlador");
        ControladorRecorrido script = Controlador.GetComponent<ControladorRecorrido>();
        if (mode == FlipMode.RightToLeft)
            script.SiguienteImagen();           
        else
            script.AnteriorImagen();


        if (mode == FlipMode.RightToLeft)
            currentPage += 2;     
        else
            currentPage -= 2;
        
        if(currentPage == 0 || currentPage>=bookPages.Length){
            notas.SetActive(false);
            titulo.SetActive(true);
        }else{
            notas.SetActive(true);
            titulo.SetActive(false);
        }
        Debug.Log("Página Actual "+currentPage+" de "+bookPages.Length);
            
        LeftNext.transform.SetParent(BookPanel.transform, true);
        Left.transform.SetParent(BookPanel.transform, true);
        LeftNext.transform.SetParent(BookPanel.transform, true);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        Right.transform.SetParent(BookPanel.transform, true);
        RightNext.transform.SetParent(BookPanel.transform, true);
        UpdateSprites();
        Shadow.gameObject.SetActive(false);
        ShadowLTR.gameObject.SetActive(false);
        if (OnFlip != null)
            OnFlip.Invoke();
    }
    public void TweenBack()
    {
        if (mode == FlipMode.RightToLeft)
        {
            currentCoroutine = StartCoroutine(TweenTo(ebr,0.15f,
                () =>
                {
                    UpdateSprites();
                    RightNext.transform.SetParent(BookPanel.transform);
                    Right.transform.SetParent(BookPanel.transform);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    pageDragging = false;
                }
                ));
        }
        else
        {
            currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f,
                () =>
                {
                    UpdateSprites();

                    LeftNext.transform.SetParent(BookPanel.transform);
                    Left.transform.SetParent(BookPanel.transform);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    pageDragging = false;
                }
                ));
        }
    }
    public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
    {
        int steps = (int)(duration / 0.025f);
        Vector3 displacement = (to - f) / steps;
        for (int i = 0; i < steps-1; i++)
        {
            if(mode== FlipMode.RightToLeft)
            UpdateBookRTLToPoint( f + displacement);
            else
                UpdateBookLTRToPoint(f + displacement);

            yield return new WaitForSeconds(0.025f);
        }
        if (onFinish != null)
            onFinish();
    }


    public void CrearPaginas(List<int> Ids){
        // El ancho de las imágenes es el 90% de la hoja
        int anchoImagen = (int)(hojaDerecha.texture.height * 0.9);

        //Forma de buscar los componenentes que tienen las imágenes
        string prefijoComponenteImagen ="ImagenRecorrido";

        // Cantidad de hojas que tendrá el libro
        int cantidadHojas = Ids.Count;

        // Se crean las páginas del libro, de acuerdo a la cantidad de páginas, sumando la cubierta y la cubierta trasera
        bookPages = new Sprite[cantidadHojas*2+2];

        //La primera página será la cuebierta
        bookPages[0]=cubierta; 

        int i = 1;
        //Se hace un ciclo sobre la cantidad de imágenes a mostrar
        foreach (int id in Ids){
        //for(int i= 1;i<=cantidadHojas;i++){

            //Se busca cada uno de los componentes que tienen las imágenes
            string nombreComponente = prefijoComponenteImagen+id;
            GameObject ContenedorImagen = GameObject.Find(nombreComponente);
            Debug.Log("Nombre -> "+nombreComponente);
            // Si se encuetra el componente se crea la hoja
            if(ContenedorImagen !=null){
                Sprite foto = ContenedorImagen.GetComponent<Image>().sprite;
                bookPages[i*2-1]=hojaIzquierda;
                bookPages[i*2]=CombinarSprites(hojaDerecha,fondoFoto,foto,cinta,anchoImagen); 
            }else{
                bookPages[i*2-1]=hojaIzquierda;
                bookPages[i*2]=hojaDerecha;
            }

            i++;
        }

        //La última hoja es la portada de Atras
        bookPages[cantidadHojas*2+1]=cubiertaAtras;
        
    }

    //Función para cambiar de tamaño una textura 2D
    private Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }

    private Sprite CombinarSprites(Sprite hojaD,Sprite fondoFoto,Sprite foto,Sprite cinta,int anchoImagen){
        
        #region Foto
        // Se saca la proporción entre el ancho y el largo de la imágen para saber si la imágen es mas ancha o larga.
        float proporcion = Convert.ToSingle(foto.texture.width)/Convert.ToSingle(foto.texture.height);
        int ancho = anchoImagen;
        int largo = anchoImagen;

        //Se cambia el parámetro ancho o largo según corresponda de acuerdo a la proporción.
        if(proporcion>1)
            largo = (int)(largo / proporcion);
            //Imagen mas ancha que larga                                   
        else
            ancho = (int)(ancho* proporcion);
        
        // Se crea el Sprite de la foto con las nuevas dimensiones.
        Sprite fotoClone =Instantiate(foto);
        Texture2D fotoCloneTexture = Instantiate(fotoClone.texture);
        fotoCloneTexture = Resize(fotoCloneTexture,ancho, largo);
        fotoClone =Sprite.Create(fotoCloneTexture,new Rect(0,0, ancho, largo),Vector2.one/2);
        #endregion
        
        #region Fondo
        //Se crea el Sprite del fondo de la foto, este tiene pixeles mas de largo y ancho de la foto.
        int pixelesDiferencia = 20;
        Sprite fondoClone = Instantiate(fondoFoto);
        Texture2D fondoCloneTexture = Instantiate(fondoFoto.texture);
        fondoCloneTexture.Resize(fotoClone.texture.width+pixelesDiferencia, fotoClone.texture.height+pixelesDiferencia);
        fondoClone =Sprite.Create(fondoCloneTexture,new Rect(0,0, fotoClone.texture.width+pixelesDiferencia, fotoClone.texture.height+pixelesDiferencia),Vector2.one/2);
        #endregion

        #region Cinta
        // La cinta tiene de ancho un 20% del ancho de la imágen
        int anchoCinta = (int)(anchoImagen*0.2);
        // Se saca la proporción entre el ancho y el largo de la cinta para saber si la cinta es mas ancha o larga.
        proporcion = Convert.ToSingle(cinta.texture.width)/Convert.ToSingle(cinta.texture.height);
        ancho = anchoCinta;
        largo = anchoCinta;

        //Se cambia el parámetro ancho o largo según corresponda de acuerdo a la proporción.
        if(proporcion>1)
            largo = (int)(largo / proporcion);
            //Imagen mas ancha que larga                                   
        else
            ancho = (int)(ancho* proporcion);
        
        // Se crea el Script de la cinta  con las nuevas dimensiones.
        Sprite cintaClone = Instantiate(cinta);
        Texture2D cintaCloneTexture = Instantiate(cintaClone.texture);
        cintaCloneTexture = Resize(cintaCloneTexture,ancho, largo);
        cintaClone =Sprite.Create(cintaCloneTexture,new Rect(0,0, ancho, largo),Vector2.one/2);
        #endregion

        //Se prepara la textura base (mainText) que será la de la hoja 
        Sprite hojaClone = Instantiate(hojaD);
        Texture2D mainTex = Instantiate(hojaClone.texture);

        //Se configura parámetros para saber desde que pixeles en x & y se va a empezar a sobreescribir el sprite
        //en el caso del fondo de la foto, como se quiere que esté centrado, es la diferencia entre la hoja y el fondo de la foto sobre dos
        int xinicial = (hojaClone.texture.width-fondoClone.texture.width)/2;
        int yinicial = (hojaClone.texture.height-fondoClone.texture.height)/2;
        
        //Se itera sobre el fondo de la imágen para ir coloreando los pixeles en la imágen base.
        for(int x = 0; x < fondoClone.texture.width; x++)
        {
            for(int y = 0; y < fondoClone.texture.height; y++){
                mainTex.SetPixel(x+xinicial,y+yinicial,fondoClone.texture.GetPixel(x,y));
            }
        }

        //Se configura parámetros para saber desde que pixeles en x & y se va a empezar a sobreescribir el sprite
        //en el caso de la foto, como se quiere que esté centrado, es la diferencia entre la hoja y la foto sobre dos
        xinicial = (hojaClone.texture.width-fotoClone.texture.width)/2;
        yinicial = (hojaClone.texture.height-fotoClone.texture.height)/2;

        //Se itera sobre la foto para ir coloreando los pixeles en la imágen base.
        for(int x = 0; x < fotoClone.texture.width; x++)
        {
            for(int y = 0; y < fotoClone.texture.height; y++)
            {
                mainTex.SetPixel(x+xinicial,y+yinicial,fotoClone.texture.GetPixel(x,y));
            }
        }

        //Se configura parámetros para saber desde que pixeles en x & y se va a empezar a sobreescribir el sprite
        xinicial = (hojaClone.texture.width-cintaClone.texture.width)/2;
        yinicial = hojaClone.texture.height-((hojaClone.texture.height-fondoClone.texture.height)/2 + (fondoClone.texture.height-fotoClone.texture.height)/4 + cintaClone.texture.height/2);

        //Se itera sobre la cinta para ir coloreando los pixeles en la imágen base.
        for(int x = 0; x < cintaClone.texture.width; x++)
        {
            for(int y = 0; y < cintaClone.texture.height; y++)
            {
                // Solo se colorea el pixel si este no tiene transparencia
                if(cintaClone.texture.GetPixel(x,y).a!=0)
                    mainTex.SetPixel(x+xinicial,y+yinicial,cintaClone.texture.GetPixel(x,y));
            }
        }
        // Se aplican los cambios sobre la imágen base y se retorna el nuevo Sprite
        mainTex.Apply();
        return Sprite.Create(mainTex, new Rect(0,0,mainTex.width,mainTex.height), Vector2.zero);
    }


    
}
