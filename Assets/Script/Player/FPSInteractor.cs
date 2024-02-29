using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    public LayerMask layerMask;
    public float maxDistance = 3f;
    public float castRadius = 0.5f;
    private Interactable _interactable;
    private Camera _fpsCamera;

    void Start()
    {
        _fpsCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey) && _interactable != null)
        {
            _interactable.Interact();
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(_fpsCamera.transform.position, castRadius, _fpsCamera.transform.forward, out hit, maxDistance, layerMask))
        {
            Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
            if (i != null)
            {
                _interactable = i;
                _interactable.DisplayText();
            }
            else
            {
                if (_interactable != null)
                    _interactable.actionText.gameObject.SetActive(false);

                _interactable = null;
            }
        }
        else
        {
            if (_interactable != null)
                _interactable.actionText.gameObject.SetActive(false);
            _interactable = null;
        }
    }
}