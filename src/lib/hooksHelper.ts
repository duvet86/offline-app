import { useRef, useState, useEffect, useCallback } from "react";

export function usePrevious<T>(value: T) {
  const ref = useRef<T>();
  useEffect(() => {
    ref.current = value;
  });
  return ref.current;
}

export function useClientRect() {
  const [rect, setRect] = useState<ClientRect>();
  const ref = useCallback((node: HTMLElement | null) => {
    if (node !== null) {
      setRect(node.getBoundingClientRect());
    }
  }, []);
  return [rect, ref] as const;
}
